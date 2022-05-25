def on_button_pressed_a():
    pass


input.on_button_pressed(Button.A, on_button_pressed_a)


def on_received_value(name, value):
    if name == "LED":
        if value == 0:
            basic.show_string("LED ON")
        else:
            basic.show_string("LED OFF")
    elif name == "FAN":
        if value == 0:
            basic.show_string("FAN ON")
        else:
            basic.show_string("FAN OFF")


radio.on_received_value(on_received_value)

radio.set_group(1)


def on_forever():
    radio.send_value("TEMP", input.temperature())
    basic.pause(5000)
    radio.send_value("LIGH", input.light_level())
    basic.pause(5000)


basic.forever(on_forever)
