import IdentificationServiceHttpClientHelper
import sys
import glob
import os
import pyaudio
import time 
from datetime import datetime
from time import gmtime, strftime
import pydub
from multiprocessing import Process
import wave
from pydub import AudioSegment
from pydub.utils import make_chunks
#pydub.AudioSegment.ffmpeg = "C:\Python34\Lib\site-packages\pydub\ffmpeg-4.0"


id1=0
key= input("Enter subscription_key ")
def create_profile(subscription_key, locale):
   helper = IdentificationServiceHttpClientHelper.IdentificationServiceHttpClientHelper(
        subscription_key)

   creation_response = helper.create_profile(locale)
   id1=creation_response.get_profile_id()
   print('Profile ID = {0}'.format(id1))
   return(id1)


'''if __name__ == "__main__":
    if len(sys.argv) < 2:
        print('Usage: python CreateProfile.py <subscription_key>')
        print('\t<subscription_key> is the subscription key for the service')
        sys.exit('Error: Incorrect Usage.')
'''
def print_all_profiles(subscription_key):
    """Print all the profiles for the given subscription key.

    Arguments:
    subscription_key -- the subscription key string
    """
    helper = IdentificationServiceHttpClientHelper.IdentificationServiceHttpClientHelper(
        subscription_key)

    profiles = helper.get_all_profiles()

    print('Profile ID, Locale, Enrollment Speech Time, Remaining Enrollment Speech Time,'
          ' Created Date Time, Last Action Date Time, Enrollment Status')
    for profile in profiles:
        print('{0}, {1}, {2}, {3}, {4}, {5}, {6}'.format(
            profile.get_profile_id(),
            profile.get_locale(),
            profile.get_enrollment_speech_time(),
            profile.get_remaining_enrollment_time(),
            profile.get_created_date_time(),
            profile.get_last_action_date_time(),
            profile.get_enrollment_status()))

'''if __name__ == "__main__":
    if len(sys.argv) < 2:
        print('Usage: python PrintAllProfiles.py <subscription_key>')
        print('\t<subscription_key> is the subscription key for the service')
        sys.exit('Error: Incorrect Usage.')'''



def get_profile(subscription_key, profile_id):
    helper = IdentificationServiceHttpClientHelper.IdentificationServiceHttpClientHelper(
        subscription_key)
    
    profile = helper.get_profile(profile_id)
    
    print('Profile ID = {0}\nLocale = {1}\nEnrollments Speech Time = {2}\nRemaining Enrollment Time = {3}\nCreated = {4}\nLast Action = {5}\nEnrollment Status = {6}\n'.format(
        profile._profile_id,
        profile._locale,
        profile._enrollment_speech_time,
        profile._remaining_enrollment_time,
        profile._created_date_time,
        profile._last_action_date_time,
        profile._enrollment_status))
        
        
'''if __name__ == "__main__":
    if len(sys.argv) < 3:
        print('Usage: python DeleteProfile.py <subscription_key> <profile_id> ')
        print('\t<subscription_key> is the subscription key for the service')
        print('\t<profile_id> the ID for a profile to delete from the sevice')
        sys.exit('Error: Incorrect usage.')'''


def enroll_profile(subscription_key, profile_id, file_path, force_short_audio):
    """Enrolls a profile on the server.

    Arguments:
    subscription_key -- the subscription key string
    profile_id -- the profile ID of the profile to enroll
    file_path -- the path of the file to use for enrollment
    force_short_audio -- waive the recommended minimum audio limit needed for enrollment
    """
   
    helper = IdentificationServiceHttpClientHelper.IdentificationServiceHttpClientHelper(
        subscription_key)

    enrollment_response = helper.enroll_profile(
        profile_id,
        file_path,
        force_short_audio == "true")
    #helper=EnrollmentResponse()
    print('Total Enrollment Speech Time = {0}'.format(enrollment_response.get_total_speech_time()))
    print('Remaining Enrollment Time = {0}'.format(enrollment_response.get_remaining_speech_time()))
    print('Speech Time = {0}'.format(enrollment_response.get_speech_time()))
    print('Enrollment Status = {0}'.format(enrollment_response.get_enrollment_status()))

'''if __name__ == "__main__":
    if len(sys.argv) < 5:
        print('Usage: python EnrollProfile.py <subscription_key> <profile_id> '
            '<enrollment_file_path>')
        print('\t<subscription_key> is the subscription key for the service')
        print('\t<profile_id> is the profile ID of the profile to enroll')
        print('\t<enrollment_file_path> is the enrollment audio file path')
        print('\t<force_short_audio> True/False waives the recommended minimum audio limit needed '
              'for enrollment')

        sys.exit('Error: Incorrect Usage.')'''
def identify_file(subscription_key, file_path, force_short_audio, profile_ids):
    """Identify an audio file on the server.

    Arguments:
    subscription_key -- the subscription key string
    file_path -- the audio file path for identification
    profile_ids -- an array of test profile IDs strings
    force_short_audio -- waive the recommended minimum audio limit needed for enrollment
    """
    try:
        helper = IdentificationServiceHttpClientHelper.IdentificationServiceHttpClientHelper(
            subscription_key)

        identification_response = helper.identify_file(
            file_path, profile_ids,
            force_short_audio == "true")
        idd=identification_response.get_identified_profile_id()
        print('Identified Speaker = {0}'.format(idd))
        print('Confidence = {0}'.format(identification_response.get_confidence()))
        return(idd)
    except AttributeError as error:
        # Output expected AttributeErrors.
        return (000000000-0000-0000-0000-0000000000)
'''if __name__ == "__main__":
    if len(sys.argv) < 5:
        print('Usage: python IdentifyFile.py <subscription_key> <identification_file_path>'
              ' <profile_ids>...')
        print('\t<subscription_key> is the subscription key for the service')
        print('\t<identification_file_path> is the audio file path for identification')
        print('\t<force_short_audio> True/False waives the recommended minimum audio limit needed '
              'for enrollment')
        print('\t<profile_ids> the profile IDs for the profiles to identify the audio from.')
        sys.exit('Error: Incorrect Usage.')'''

def delete_profile(subscription_key, profile_id):
		""" Deletes a profile from the server
		
		Arguments:
		profile_id -- the profile ID string of user to delete
		"""
		helper = IdentificationServiceHttpClientHelper.IdentificationServiceHttpClientHelper(
        subscription_key)
		helper.delete_profile(profile_id)
		
		print('Profile {0} has been successfully deleted.'.format(profile_id))
    
'''if __name__ == "__main__":
    if len(sys.argv) < 3:
        print('Usage: python DeleteProfile.py <subscription_key> <profile_id> ')
        print('\t<subscription_key> is the subscription key for the service')
        print('\t<profile_id> the ID for a profile to delete from the sevice')
        sys.exit('Error: Incorrect usage.')'''

def check(f,p):
    e=str
    for line in f:
        s=line.split(':')
        e=s[0]
        d=s[1]
        if(d==p):
            return e

def record():
    FORMAT = pyaudio.paInt16
    CHANNELS = 1
    RATE = 16000
    CHUNK = 1024
    RECORD_SECONDS = 10
    WAVE_OUTPUT_FILENAME = "start.wav"
    audio = pyaudio.PyAudio()
    
    stream = audio.open(format=FORMAT, channels=CHANNELS,
        rate=RATE, input=True,  
        frames_per_buffer=CHUNK)
    print("recording...")
    frames = []
    for i in range(0, int(RATE / CHUNK * RECORD_SECONDS)):
        data = stream.read(CHUNK)
        frames.append(data)
    
    print("finished recording")

    # stop Recording
    stream.stop_stream()
    stream.close()
    audio.terminate()
 
    waveFile = wave.open(WAVE_OUTPUT_FILENAME, 'wb')
    waveFile.setnchannels(CHANNELS)
    waveFile.setsampwidth(audio.get_sample_size(FORMAT))
    waveFile.setframerate(RATE)
    waveFile.writeframes(b''.join(frames))
    waveFile.close()    
    
def split():
    x=list()
    short_audio="true"
    '''myaudio = AudioSegment.from_file("start.wav" , "wav") 
    chunk_length_ms =  # pydub calculates in millisec
    chunks = make_chunks(myaudio, chunk_length_ms) #Make chunks of one sec'''
    #Export all of the individual chunks as wav files
    '''for i, chunk in enumerate(chunks):
        chunk_name = "chunk{0}.wav".format(i)
        print("exporting", chunk_name)
        chunk.export(chunk_name, format="wav")'''
    
    '''for filename in glob.glob('chunk*.wav'):
        print(filename)'''
    p=str("C:\\Python34\\start.wav")
    '''path=p+start.wav'''
    print(p)
    f=open("Name.txt","r")
    x=[]
    for line in f:
        s=line.split(':')
        s[1]=s[1].rstrip("\n")
        x.append(s[1])
    #print(x)
    f.close()
    pid1=identify_file(key,p,short_audio,x)
    if(pid1==00000000-0000-0000-0000-0000000000):
        str2="no user"
        print(str2)
        #exit(0)
    else:
        f1=open("Name.txt","r")
        str2=check(f1,pid1)
        print(str2)
        f1.close()
        f2=open("speaker.txt","a")
        str3=str(datetime.now())
        f2.writelines(str2+" : "+str3+"\n")
        f2.close()


c=chr
now=time.time()
str1=str
str2=str
c='Y'
while c=='Y':
    ch=int(input("enter choice \n 1.Create  a profile \n 2.Get Profile \n 3.Print all Profiles \n 4.Enroll audio to profile \n 5.Delete Profile \n 6.Identify Audio"))
    if(ch==1):
        str1=input("enter name of person")
        p_idd=create_profile(key,'en-us')
        f=open("Name.txt",'a')
        f.writelines(str1+":"+p_idd+"\n")
        f.close()
    elif(ch==2):
        p_id=input("enter profile id")
        get_profile(key,p_id)
    elif(ch==3):
	    print_all_profiles(key)
    elif(ch==4):
        p_id=input("enter profile id")
        path=input("enter file path")
        short_audio="true"
        enroll_profile(key,p_id,path,short_audio)
    elif(ch==5):
        p_id=input("enter profile id")
        delete_profile(key,p_id)
    elif(ch==6):
        '''future=now+50
        while(time.time()<future):
            record()
            split()'''
        timeout = time.time() + 60*1
        while(time.time() < timeout):
                record()
                split()
    c=input("Do u wish to continue Y/N?")








