import IdentificationServiceHttpClientHelper
import sys
import glob
import os
import pyaudio
import threading
import os
from threading import Thread
import time 
from datetime import datetime,timedelta,date,time
from time import gmtime, strftime
import pydub
import wave
from pydub import AudioSegment
from pydub.utils import make_chunks

id1=0
a = 1;
c=chr
#now=time.time()
str1=str
str2=str
FORMAT = pyaudio.paInt16
CHANNELS = 1
RATE = 16000
CHUNK = 1402
RECORD_SECONDS = 20
WAVE_OUTPUT_FILENAME = "start.wav"
audio = pyaudio.PyAudio()
    
stream = audio.open(format=FORMAT, channels=CHANNELS,
        rate=RATE, input=True,  
        frames_per_buffer=CHUNK)


key= "761ef79c3ed342f6bd5b2e7e88804d0f"
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

def check(f,p):
    e=str
    flag=0
    str4=str
    for line in f:
        s=line.split(':')
        e=str(s[0])
        s[1]=s[1].rstrip("\n")
        d=str(s[1])
        if(d != p):
            continue
        else:
            flag=1
            return e   
    if(flag!=1):
        str4="new_speaker"
        return str4

def recog(f,time):
    x=list()
    #print(time)
    #pid1=str
    short_audio="true"
    
    p=str(f)
    '''path=p+start.wav'''
    #print(p)
    f=open("Name.txt","r")
    x=[]
    for line in f:
        s=line.split(':')
        s[1]=s[1].rstrip("\n")
        x.append(s[1])
    #print(x)
    f.close()
    pid1=identify_file(key,p,short_audio,x)
    if(pid1 == "00000000-0000-0000-0000-000000000000"):
        str2="nouser"
        #print(str2)
        f2=open("speaker.txt","a")
        str3=time.strftime("%H:%M:%S")
        f2.writelines(str2+" "+str3+" ")
        f2.close()
        #exit(0)
    else:
        f1=open("Name.txt","r")
        str2=check(f1,pid1)
        #print(str2)
        f1.close()
        f2=open("speaker.txt","a")
        #str3=str(datetime.now())
        str3=time.strftime("%H:%M:%S")
        f2.writelines(str2+" "+str3+" ")
        f2.close()

def addSecs(tm, secs):
    #fulldate = datetime.datetime(tm.hour, tm.minute, tm.second)
    fulldate=tm
    fulldate = fulldate+timedelta(seconds=secs)
    #print(fulldate)
    return(fulldate)


def sampling(start_time):
    myaudio = AudioSegment.from_file("start.wav" , "wav") 
    chunk_length_ms = 3000 # pydub calculates in millisec
    chunks = make_chunks(myaudio, chunk_length_ms) 
    #Make chunks of one sec
    #Export all of the individual chunks as wav files
    chunk_time=start_time
    for i, chunk in enumerate(chunks):
      
        #print(chunk_time)
        chunk_name = "chunk{0}.wav".format(i)
        #print("exporting", chunk_name)
        chunk.export(chunk_name, format="wav")
        recog(chunk_name,chunk_time)
        chunk_time=addSecs(chunk_time,10)
        '''for filename in glob.glob('chunk*.wav'):
        print(filename)
        recog(filename)'''
    

def record():
    
    print("recording...")
    #start_time=datetime.now()
    global start_time
    global a
    start_time = datetime.now()
    #start_time=str(start_time1) 
    global frames
    frames = []
    for i in range(0, int(RATE / CHUNK * RECORD_SECONDS)):
        data = stream.read(CHUNK)
        frames.append(data)
        if os.path.exists("stopper.txt"):
            a=0 
            break              
    a=0        
    print("finished recording")

t1 = threading.Thread(target=record)

print("Starting thread")
t1.start()
while(a != 0 ):
    continue
#if os.path.exists("stopper.txt"):
    #os.remove("stopper.txt")
try:
    t1.terminate()
except Exception:
    big=0
stream.stop_stream()
stream.close()
audio.terminate()
waveFile = wave.open(WAVE_OUTPUT_FILENAME, 'wb')
waveFile.setnchannels(CHANNELS)
waveFile.setsampwidth(audio.get_sample_size(FORMAT))
waveFile.setframerate(RATE)
waveFile.writeframes(b''.join(frames))
waveFile.close() 
print(a) 
sampling(start_time)