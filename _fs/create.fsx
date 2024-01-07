#load "../../../__lib/fs/web_create.fsx"
//#load "/Users/ching/Documents/GoogleDrive/My Drive/__lib/fs/web_create.fsx"
//#load @"C:\Dropbox\__lib\fs/web_create.fsx"

let title = "Tailor Your Ifc with openbim-components."
let path = __SOURCE_DIRECTORY__
title |> Web_create.newPost path
