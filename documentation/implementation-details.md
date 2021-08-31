# Implementation Details

This file contains implementation details for the Domain Modeling 
Assistant application.

## System Architecture

![architecture](architecture.png)

_TODO: Make a more formal figure and add descriptions._

## Control Flow

The control flow of the application is illustrated with the following
example that shows how to add a class.

1. The user selects the `Add Class` button in the frontend.
This button is associated with
[`AddClassAction.AddClass()`](../domain-model-assistant/Assets/Components/Scripts/AddClassAction.cs)
in the Unity Editor.
1. `AddClass()` calls
[`Diagram.AddClassButtonPressed()`](../domain-model-assistant/Assets/Components/Scripts/Diagram.cs#L366),
which activates the `AddingClass` mode, where a single click
on the canvas will add a class. The cursor has a `+` in this mode,
visible in the browser (but not the Unity Editor) thanks to 
the `SetCursorToAddMode()` function defined in
[`cdmeditor.jslib`](../domain-model-assistant/Assets/Plugins/cdmeditor.jslib).
1. When the user clicks on the canvas, at the next frame
`Diagram.Update()` will run and detect the click and call
`Diagram.AddClass()` with suitable arguments.
1. `Diagram.AddClass()` makes a POST request to the WebCORE with the
class to be added. The data is sent as a JSON object. Immediately
after the request is sent, another (GET) request is made to WebCORE
to fetch the updated diagram.
1. Since we know the diagram should change, `Diagram.Update()` will
keep checking whether the `_getRequestAsyncOp.isDone`, and if so,
it will call `Diagram.LoadJson()` with the updated diagram data.
1. `Diagram.LoadJson()` will deserialize the JSON data into a
`ClassDiagramDTO` and map each class `_id` to a (class object, position) 
pair, eg, `14 -> [<Class with name "Airplane">, Vector2(50, 100)]`.
It will then `CreateCompartmentedRectangle()` for each class, while 
saving its name that will be otherwise be overwritten (all classes would
have the same name, of the most recently added class).
1. Since `_namesUpToDate` is false, `Diagram.UpdateNames` will be called
by `Diagram.LateUpdate()`, which runs after `Update()`.
1. The updated diagram is then shown to the user. This should take only a
a few milliseconds.

The one thing not mentioned above is how an external webpage could 
communicate with the frontend application. The inner
[`index.html`](../unity-webgl-output/index.html) has been modified
to expose `unity` to (trusted) external scripts, eg, the outer
[`index.html`](../index.html)], which contacts the former using the
`postMessage()` function.




