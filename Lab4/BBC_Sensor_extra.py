def on_received_string(receivedString):
    global list2
    if not (receivedString.includes("#")):
        list2 = receivedString.split(":")
        if parse_float(list2[0]) == id2:
            if parse_float(list2[1]) == 0:
                basic.show_string("" + (list2[1]))
            else:
                basic.show_string("" + (list2[1]))


radio.on_received_string(on_received_string)

list2: List[str] = []
id2 = 0
radio.set_group(1)
id2 = 1


def on_forever():
    radio.send_string("!" + str(id2) + ":TEMP:" +
                      str(input.temperature()) + "#")
    basic.pause(5000)
    radio.send_string("!" + str(id2) + ":LIGHT:" +
                      str(input.light_level()) + "#")
    basic.pause(5000)


basic.forever(on_forever)
