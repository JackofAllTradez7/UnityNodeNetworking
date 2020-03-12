var mongoose = require("mongoose")
var Schema = mongoose.Schema

var UserSchema = new Schema
({
    name:
    {
        type:String,
        required:true
    },
    score:
    {
        type:String,
        required:true
    }
})

mongoose.model("user", UserSchema)