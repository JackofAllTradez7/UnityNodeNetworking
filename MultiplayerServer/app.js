var io = require("socket.io")(process.env.PORT || 3000)
var shortid = require("shortid") //npm install shortid --save
var db = require("./database")
var mongoose = require("mongoose")
var app = require("express")
require("./models/User")
var user = mongoose.model("user")

mongoose.connect(db.mongoURI,
    {
        useNewUrlParser:true,
        useUnifiedTopology:true
    }).then(function(){
        console.log("MongoDB connected")
    }).catch(function(err)
    {
        console.log(err);
    });

//console.log(shortid.generate())

console.log("Server Connected")
var players = [];
io.on("connection", function(socket)
{
    
    console.log("Client Connected")

    var thisClientId = shortid.generate();
    players.push(thisClientId)   

    //for new players
    socket.broadcast.emit("spawn", {id:thisClientId})

    // req logged in players positions
    socket.broadcast.emit("requestPosition")

    players.forEach(function(playerID)
    {
        if(playerID == thisClientId)
        {
            return
        }
        socket.emit("spawn", {id:playerID})
        console.log("spawningplayer of  I=id: ", playerID)
    })

    socket.on("Jeff", function(data)
    {
        console.log("Name always Jeff")
        console.log(data)
    })

    socket.on("updatePosition", function(data)
    {
        data.id = thisClientId
        socket.broadcast.emit("updatePosition",  data)
    })

    socket.on("score", function(data)
    {
        console.log("THIS IS CHECKING THE SCORE")
        user.findOne({name:data.name}).then(function(user)
        {
            console.log("score updating")
          user.score = data.score
          user.save().then(function(epicandcool)
          {
                console.log("updated")
          })  
        }).catch(function(err)
        {
            var newUser =
            {
                name:data.name,
                score:data.score
            }
            new user(newUser).save().then(function(user)
            {
                console.log("woah")
            })
        })
        console.log("THIS IS THE FINAL SCORE UPDATE")
    })

    socket.on("move", function(data)
    {
        data.id = thisClientId;
        console.log("Player is schmooving", JSON.stringify(data))
        socket.broadcast.emit("move", data)
    })

    socket.on("disconnect", function()
    {
        console.log("Player Disconnected")
        players.splice(players.lastIndexOf(thisClientId), 1)
        socket.broadcast.emit("disconnected", {id:thisClientId})
    })
})