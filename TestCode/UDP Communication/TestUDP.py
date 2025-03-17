import socket

<<<<<<< HEAD
UDP_IP = "192.168.14.196"
UDP_PORT = 11069
MESSAGE = """1,25,0,0"""

print ("UDP target IP:", UDP_IP)
print ("UDP target port:", UDP_PORT)
print ("message:", MESSAGE)

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
sock.sendto(MESSAGE.encode(), (UDP_IP, UDP_PORT))
