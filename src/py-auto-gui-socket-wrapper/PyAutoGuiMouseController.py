
import sys
import os
import pyautogui
import socket

port = 60606




def HandleCommandCall(command):

    args = command.split()
    #print("full arg:", args)
    knownCommands = {
        "-mo":1, # move mouse
        "-cl":2, # click left mouse button
        "-ld":3, # left mouse button down
        "-lu":4, # left mouse button up
        "-cr":5, # click right mouse button
        "-cd":6, # click double?
        "-sd":7, # scroll down
        "-su":8, # scroll up
        "-lo":9  # location of mouse
    }

    command = knownCommands[args[0]]
    #print("number of args: ", len(args))
    # switch
    if (knownCommands["-mo"] == command):
        #get further data from sys args
        #splitting on space
        print("trying to move", args)
        if(3 != len(args)):
            return 1
        x = int(args[1])
        y = int(args[2])
        #move the mouse
        pyautogui.moveTo(x,y,0)
        print("finished moving")
        return 0
    elif (knownCommands["-cl"] == command):
        #left click
        pyautogui.leftClick()
        return 0
    elif (knownCommands["-ld"] == command):
        #left mouse down
        pyautogui.mouseDown()
        return 0;
    elif (knownCommands["-lu"] == command):
        #left mouse up
        pyautogui.mouseUp()
        return 0
    elif (knownCommands["-cr"] == command ):
        #click right mouse button
        pyautogui.rightClick()
        return 0
    elif (knownCommands["-cd"] == command):
        #double click
        pyautogui.doubleClick()
        return 0
    elif (knownCommands["-sd"] == command):
        pyautogui.scroll(clicks=-125)
        return 0
    elif (knownCommands["-su"] == command):
        pyautogui.scroll(clicks=125)
        return 0
    elif (knownCommands["-lo"] == command):
        #get location of mouse
        currentMouseX, currentMouseY = pyautogui.position()
        print(currentMouseX," ",currentMouseY,end='')
        return 0
    #end of switch
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
    print ('Got connection from', addr)

    # send a thank you message to the client.
    while True:
        lengthOfData = c.recv(1)
        print(int(lengthOfData[0]))
        data = c.recv(int(lengthOfData[0]))
        params = data.decode("utf-8")
        print(params)
        HandleCommandCall(params)
    # Close the connection with the client
    c.close()