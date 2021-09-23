#!/usr/bin/env node

const express = require('express');
const app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

var classDiagram = {
  "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//ClassDiagram",
  "_id": "100",
  "name": "MULTIPLE_CLASSES",
  "classes": [
    {
      "eClass": "http://cs.mcgill.ca/sel/cdm/1.0#//Class",
      "_id": "1",
      "name": "ClassOne"
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
        "values": [
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
app.get('/classdiagram/MULTIPLE_CLASSES', function (req, res) {
  console.log(classDiagram);
  res.json(classDiagram); // TODO change
});

// Add class
app.post('/classdiagram/MULTIPLE_CLASSES/class', function (req, res) {
  console.log("1:" + JSON.stringify(req.body));
  // TODO

  res.sendStatus(200);
});

// Delete class
app.delete('/classdiagram/MULTIPLE_CLASSES/class/:class_id', function (req, res) {
  const classId = req.params.class_id;
  console.log(JSON.stringify(req.body));
  const allClassIds = classDiagram.classes.map(c => c._id);
  var indexToRemove = allClassIds.indexOf(classId);
  if (indexToRemove > -1) {
    classDiagram.classes.splice(indexToRemove, 1);
  }
  const allLayoutIds = classDiagram.layout.containers[0].values.map(c => c.key);
  var indexToRemove2 = allLayoutIds.indexOf(classId);
  if (indexToRemove2 > -1) {
    classDiagram.layout.containers[0].values.splice(indexToRemove2, 1);
  }
  res.sendStatus(200);
});

var server = app.listen(8080, function () {
  var host = server.address().address
  var port = server.address().port
  console.log("Example app listening at http://%s:%s", host, port)
});
