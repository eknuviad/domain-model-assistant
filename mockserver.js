#!/usr/bin/env node

/*
 * Mock server used instead of WebCORE to test the Unity code.
 * 
 * Setup:
 * Install/update `node` and `npm` if needed, then run `npm i` to install dependencies
 * 
 * Run:
 * `node mockserver.js` or `./mockserver.js` on *nix
 */

const express = require('express');
const app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
const cors = require("cors");
const corsOptions = {
  origin: ['http://localhost:8080', 'http://127.0.0.1'],
  methods: ['GET', 'POST', 'DELETE', 'UPDATE', 'PUT', 'PATCH'],
  credentials: true,            //access-control-allow-credentials:true
  optionSuccessStatus: 200,
}

app.use(cors(corsOptions)) // Use this after the variable declaration

const SUCCESS = 200;
const PORT = 8080;

var classDiagram = {
  "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//ClassDiagram",
  "_id": "100",
  "name": "MULTIPLE_CLASSES",
  "classes": [
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//Class",
      "_id": "1",
      "name": "Class1",
      "attributes": [{
        "_id": "2",
        "name": "year",
        "type": "6"
      }, {
        "_id": "5",
        "name": "month",
        "type": "8"
      }]
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//Class",
      "_id": "2",
      "name": "Class2"
    }
  ],
  "types": [
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDVoid",
      "_id": "2"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDAny",
      "_id": "3"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDBoolean",
      "_id": "4"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDDouble",
      "_id": "5"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDInt",
      "_id": "6"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDLong",
      "_id": "7"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDString",
      "_id": "8"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDByte",
      "_id": "9"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDFloat",
      "_id": "10"
    },
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//CDChar",
      "_id": "11"
    }
  ],
  "layout": {
    "_id": "12",
    "containers": [
      {
        "_id": "13",
        "key": "null",
        "value": [ // TODO Change to "values" when WebCORE is updated
          {
            "_id": "14",
            "key": "1", // is the same as the class id
            "value": {
              "_id": "15",
              "x": 365.5,
              "y": 300.0
            }
          },
          {
            "_id": "15",
            "key": "2",
            "value": {
              "_id": "105",
              "x": 565.5,
              "y": 180.0
            }
          }
        ]
      }
    ]
  }
};

//TODO: unsure what these ids are for, hard coded for now
var valueId = 16;
var valueValueId = 106;

// GET class diagram
app.get('/classdiagram/MULTIPLE_CLASSES', (req, res) => {
  console.log(classDiagram);
  res.json(classDiagram); // TODO change
  // res.sendStatus(SUCCESS);
});

// Add class
app.post('/classdiagram/MULTIPLE_CLASSES/class', (req, res) => {
  const className = req.body.className;
  const xPos = req.body.x;
  const yPos = req.body.y;

  const allClassIds = classDiagram.classes.map(c => c._id);
  const newClassId = (parseInt(allClassIds[allClassIds.length - 1]) + 1).toString();

  classDiagram.classes.push({
    "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//Class",
    "_id": newClassId,
    "name": className
  })

  classDiagram.layout.containers[0].value.push({
    "_id": valueId,
    "key": newClassId,
    "value": {
      "_id": valueValueId,
      "x": xPos,
      "y": yPos
    }
  })

  valueId += 1;
  valueValueId += 1;

  console.log(">>> Adding class given req.body: " + JSON.stringify(req.body));

  res.sendStatus(SUCCESS);
});

//Update class position
app.put('/classdiagram/MULTIPLE_CLASSES/:classId/position', (req, res) => {
  const classId = req.params.classId;
  var values = classDiagram.layout.containers[0].value/*s*/; // TODO Change to "values" later
  // retrieve name of class with updated position
  const allClassIds = classDiagram.classes.map(c => c._id);
  var classIndex = allClassIds.indexOf(classId);
  var className = classDiagram.classes[classIndex].name;
  //update position details
  const allLayoutIds = values.map(c => c.key);
  var index = allLayoutIds.indexOf(classId);
  var value = classDiagram.layout.containers[0].value[index].value;
  value.x = req.body.xPosition;
  value.y = req.body.yPosition;
  console.log(`>>> Updated ${className} position = x: ${value.x}, y: ${value.y}`);
  res.sendStatus(SUCCESS);
});

// Delete class
app.delete('/classdiagram/MULTIPLE_CLASSES/class/:class_id', (req, res) => {
  const classId = req.params.class_id;
  const allClassIds = classDiagram.classes.map(c => c._id);
  var indexToRemove = allClassIds.indexOf(classId);
  if (indexToRemove > -1) {
    classDiagram.classes.splice(indexToRemove, 1);
  }

  var values = classDiagram.layout.containers[0].value/*s*/; // TODO Change to "values" later
  const allLayoutIds = values.map(c => c.key);
  var indexToRemove2 = allLayoutIds.indexOf(classId);
  if (indexToRemove2 > -1) {
    values.splice(indexToRemove2, 1);
  }
  res.sendStatus(SUCCESS);
});

// Add attribute
app.post('/classdiagram/MULTIPLE_CLASSES/class/:classId/attribute', (req, res) => {
  const attributeName = req.body.attributeName;
  const rankIndex = req.body.rankIndex;
  const typeId = req.body.typeId;
  // @param body {"rankIndex": Integer, "typeId": Integer, "attributeName": String}

  console.log(">>> Adding attribute given req.body: " + JSON.stringify(req.body));
  // console.log(JSON.stringify(myObject, null, 4));

  res.sendStatus(SUCCESS);
});

var server = app.listen(PORT, () => {
  var host = server.address().address
  var port = server.address().port
  console.log("Example app listening at http://%s:%s", host, port)
});
