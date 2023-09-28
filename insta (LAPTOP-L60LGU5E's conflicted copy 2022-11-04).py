import pyautogui, time 
time.sleep(5)
f = open("Big.txt" , "r")

for word in f: 
    pyautogui.typewrite(word)
    pyautogui.press("enter") 
    

