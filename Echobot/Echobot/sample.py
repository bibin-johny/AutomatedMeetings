import sys
if __name__ == '__main__':
    f = open("C:\\Users\\vishnu\\Desktop\\text.txt", 'r')
    s = f.read()
    f.close()
    print (s)
    f = open("text2.txt", 'w')
    f.write(s)
    f.close()