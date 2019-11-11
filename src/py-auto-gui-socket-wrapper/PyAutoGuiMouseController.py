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
        "-lo":9,  # location of mouse
        "-rd":10, # right mouse button down
        "-ru": 11,  # right mouse button down

    }

    command = knownCommands[args[0]]
    #print("number of args: ", len(args))
    # switch
    fromLeft = -1
    fromTop = -1
    if len(args) == 3:
        fromLeft = int(args[1])
        fromTop = int(args[2])

    if (knownCommands["-mo"] == command):
        #get further data from sys args
        #splitting on space
        #print("trying to move", args)
        if(3 != len(args)):
            return 1
        #move the mouse
        pyautogui.moveTo(x,y,0)
        #print("finished moving")
        return 0
    elif (knownCommands["-cl"] == command):
        #left click
        pyautogui.leftClick(x = fromLeft, y= fromTop)
        return 0
    elif (knownCommands["-ld"] == command):
        #left mouse down
        pyautogui.mouseDown( x = fromLeft, y= fromTop)
        return 0;
    elif (knownCommands["-lu"] == command):
        #left mouse up
        pyautogui.mouseUp( x = fromLeft, y= fromTop)
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
        print(currentMouseX," ",currentMouseY,end= '')
        return 0
    elif (knownCommands["-rd"] == command):
        #get location of mouse
        currentMouseX, currentMouseY = pyautogui.position()
        pyautogui.mouseDown(button="right", x = fromLeft, y= fromTop)
        return 0
    elif (knownCommands["-ru"] == command):
        #get location of mouse
        currentMouseX, currentMouseY = pyautogui.position()
        pyautogui.mouseUp(button="right", x = fromLeft, y= fromTop)
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
    print ('Python mouse controller got connection from', addr)

    # send a thank you message to the client.
    while True:
        lengthOfData = c.recv(1)
        print(int(lengthOfData[0]))
        data = c.recv(int(lengthOfData[0]))
        params = data.decode("utf-8")
        print("Python mouse controller received command: " + params)
        HandleCommandCall(params)
    # Close the connection with the client
    c.close()



