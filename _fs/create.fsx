//--#load "/Users/ching/Documents/Dropbox/__lib/fs/web_create.fsx"
//#load "/Users/ching/Documents/GoogleDrive/My Drive/__lib/fs/web_create.fsx"
#load @"C:\Dropbox\__lib\fs/web_create.fsx"

let title = "Release tyRCore Version 0.9"
let path = __SOURCE_DIRECTORY__
title |> Web_create.newPost path

