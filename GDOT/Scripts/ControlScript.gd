extends Node


var TC_Last = 1
var TimeCompressionStep = 1.0
var TimeCompKeyDebounce = 0

var Notify
var NotifyTimer = 0

var Info
var LastInfo = ""

func _input(event):
	
	if event is InputEventKey and event:
		
		var TCKD = -1
		match event.keycode:
			KEY_COMMA:
				TimeCompressionStep = .1
				TCKD = .5
			KEY_PERIOD:
				TimeCompressionStep = 10
				TCKD = .5
			KEY_SLASH:
				TimeCompressionStep = 0
				TimeCompKeyDebounce = TCKD
		if (TimeCompKeyDebounce > 0):
			TimeCompressionStep = 1
		if (TimeCompKeyDebounce < 0):
			TimeCompKeyDebounce = TCKD
		
		

func SignalNotify(string): #make this async
	Notify.visible = true
	Notify.get_node("Title").text = string
	NotifyTimer = 5
func SignalInfo(): #make this async
	Info.visible = true
	Info.get_node("Title").text = get_meta("InfoText")
	Info.get_node("Status").text = get_meta("InfoStatus")
	
	NotifyTimer = 10

# Called when the node enters the scene tree for the first time.
func _ready():
	Notify = get_node("Notify")
	Info = get_node("Info")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	NotifyTimer-=delta
	TimeCompKeyDebounce -= delta
	if (NotifyTimer <= 0):
		Notify.visible = false
		Info.visible = false
	var TimeCompCurrent = get_meta("TimeCompression")
	if (TimeCompressionStep != 1):
		TimeCompCurrent *= TimeCompressionStep
		if (TimeCompCurrent == 0) and (TimeCompressionStep != 0):
			TimeCompCurrent = 1
		if (TimeCompCurrent < 1.0):
			TimeCompCurrent = 0
			
		set_meta("TimeCompression", int(TimeCompCurrent))
		
	if (TC_Last != TimeCompCurrent):
		SignalNotify(" TIME COMPRESSION: " + str(get_meta("TimeCompression")) + "X")
		if (TimeCompCurrent == 0):
			SignalNotify("PAUSED ||")
			NotifyTimer = 9999
		if (TimeCompCurrent == 1):
			SignalNotify("REALTIME")
	if (LastInfo != get_meta("InfoText")):
		SignalInfo()
	TimeCompressionStep = 1
	
	TC_Last = TimeCompCurrent
	LastInfo = get_meta("InfoText")
	pass
