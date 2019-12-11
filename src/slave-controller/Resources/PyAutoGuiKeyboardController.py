import os

import pyautogui
import socket

port = 60600




def HandleCommandCall(command):

    parts = command.split("#@!")
    key = parts[0].split("!@#")[1]
    isDown = "true" == (parts[1].split("!@#")[1])

    if(isDown):
        pyautogui.keyDown(key)
        return 0
    else:
        pyautogui.keyUp(key)
        return 0

    return 1

serverSocket = socket.socket()
print ("Socket successfully created")

serverSocket.bind(('',port))
print ("socket binded to: " ,port)
serverSocket.listen(5)


# will only accept one tcp connection at a time
while True:
    # Establish connection with client.

    c, addr = serverSocket.accept()
    print ('Python keyboard controller got connection from', addr)

    # send a thank you message to the client.
    while True:
        lengthOfData = c.recv(1)
        print(int(lengthOfData[0]))
        data = c.recv(int(lengthOfData[0]))
        params = data.decode("utf-8")
        print("Python keyboard controller received command: " + params)
        HandleCommandCall(params)
    # Close the connection with the client
    c.close()
