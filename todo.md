# TODO List

## 17 July 2021

- Refactor `ClassDiagramDTO` and update the associated JSON with feedbacks
- Update `index.html` template in Unity settings with `temp.js` content to expose Unity instance to browser
- Call this Unity instance from parent Modeling Assistant page
- Add a JSON textbox to MA page and a `Show JSON` button
- Parameterize `Diagram.Update()`, which handles double-clicks, to draw a rectangle at the double
click location (this depends on extracting a helper function to draw a `Class` at a given `Coordinate`)
