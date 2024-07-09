#load "../../../__lib/fs/web_create.fsx"

//#load "/Users/ching/Documents/GoogleDrive/My Drive/__lib/fs/web_create.fsx"
//#load @"C:\Dropbox\__lib\fs/web_create.fsx"

let title = "Ask our GPT About F# Coding for Revit"
let path = __SOURCE_DIRECTORY__
title |> Web_create.newPost path
