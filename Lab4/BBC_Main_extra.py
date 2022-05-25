def on_received_string(receivedString):
    serial.write_string(receivedString)
    basic.show_string(receivedString)


radio.on_received_string(on_received_string)


def on_data_received():
    global cmd
    cmd = serial.read_until(serial.delimiters(Delimiters.HASH))
    if cmd == "0":
        radio.send_string("1:" + "0")
    elif cmd == "1":
        radio.send_string("1:" + "1")
    elif cmd == "2":
        radio.send_string("2:" + "0")
    elif cmd == "3":
        radio.send_string("2:" + "1")


serial.on_data_received(serial.delimiters(Delimiters.HASH), on_data_received)

cmd = ""
radio.set_group(1)
