# TODO List

## 17 July 2021

- Refactor `ClassDiagramDTO` and update the associated JSON
- Update `index.html` template in Unity settings with `temp.js` content to expose Unity instance to browser
- Call this Unity instance from parent Modeling Assistant page
- Add a JSON textbox to MA page and a `Show JSON` button
- Add a quick command textbox to MA page and a `Run Command` button to allow commands in the form
  ```js
  addClass("Driver", 12, 45)
  ``` 
- Parameterize `Diagram.Update()`, which handles double-clicks, to draw a rectangle at the double
click location (this depends on extracting a helper function to draw a `Class` at a given `Coordinate`)
