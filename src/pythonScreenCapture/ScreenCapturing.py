
import sys
import os
import pyautogui
import socket
import threading
import queue
import socket
import time

# this program will make screnshots, and put them in a sending queue. if there are no images in the sending queue the
# sending thread will wait and continiouslt check the queue. if there are more than two images in the queue these will
# be dequeued and not send
print(sys.executable)

print(sys.argv)

offsetFromLeft = int(sys.argv[1])
offsetFromTop = int(sys.argv[2])
imageWidth = int(sys.argv[3])
imageHeight = int(sys.argv[4])

ourQueue = queue.Queue()

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
#print("trying to connect")
s.bind(('', 30303))
#s.connect(('10.152.210.13', 30303)) # krystof ip
print('Python image capturer ready to accept a connection')

def threadCaptureScreen():
    if not os.path.exists('img'):
        os.makedirs('img')
    print("Python capture screen thread started")
    while(True):
        for i in range(100):
            fileName = "img/" + str(i) + ".jpg"
            pyautogui.screenshot( fileName, region=(offsetFromLeft,offsetFromTop,imageWidth,imageHeight) )
           # pyautogui.screenshot()
            ourQueue.put(fileName)

def threadSendToClient():
    print("Python send to client thread started")
    s.listen(10)  # socket now listening
    while(True):
        try:
            conn,addr = s.accept()
            while(True):
                while(0 == ourQueue.qsize()):
                    time.sleep(0.01)
                    #print("sleeping because there are no images in the queue")
                while(ourQueue.qsize() > 2):
                    ourQueue.get()
                    #print("dumping images because queue is too big")
                fileName = ourQueue.get()
                #print("file name read from queue:" + fileName)
                in_file = open(fileName, "rb")  # opening for [r]eading as [b]inary
                data = in_file.read()  # if you only wanted to read 512 bytes, do .read(512)
                in_file.close()
                #print("image size is: " + str(len(data)))
                theImageSize = (len(data)).to_bytes(4, byteorder="little", signed=True)
                #print("image size in bytes: " + str(theImageSize))
                conn.send(theImageSize)
                sentData = conn.send(data)
                #print('The amount of the data that was sent: ' + str(sentData))
                print("Python finished sending a file to the client")
        except Exception:
            print(Exception)


threading.Thread(target=threadCaptureScreen).start()
threading.Thread(target=threadSendToClient).start()

print("\nPython started worker threads and putting main thread to sleep in a loop")

while(True):
    time.sleep(0.1)



