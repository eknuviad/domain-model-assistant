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
      "name": "ClassOne",
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
      "_id": "102",
      "name": "ClassTwo"
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
              "y": 80.0
            }
          },
          {
            "_id": "103",
            "key": "102",
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

// GET class diagram
app.get('/classdiagram/MULTIPLE_CLASSES', (req, res) => {
  console.log(classDiagram);
  res.json(classDiagram); // TODO change
  // res.sendStatus(SUCCESS);
});

// Add class
app.post('/classdiagram/MULTIPLE_CLASSES/class', (req, res) => {
  console.log(">>> Adding class given req.body: " + JSON.stringify(req.body));
  // TODO

  res.sendStatus(SUCCESS);
});

//Update class position
app.put('/classdiagram/MULTIPLE_CLASSES/:classId/position', (req, res) => {
  const classId = req.params.classId;
  var values = classDiagram.layout.containers[0].value/*s*/; // TODO Change to "values" later
  const allLayoutIds = values.map(c => c.key);
  var index = allLayoutIds.indexOf(classId);
  classDiagram.layout.containers[0].value[index].value.x = req.body.xPosition;
  classDiagram.layout.containers[0].value[index].value.y = req.body.yPosition;
  console.log(">>> Updated classDiagram position = x: " + classDiagram.layout.containers[0].value[index].value.x
    + "y:" + classDiagram.layout.containers[0].value[index].value.y);
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

var server = app.listen(PORT, () => {
  var host = server.address().address
  var port = server.address().port
  console.log("Example app listening at http://%s:%s", host, port)
});
