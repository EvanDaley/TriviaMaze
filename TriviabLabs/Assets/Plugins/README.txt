Hey guys,
Don't move / delete anything in this folder! It was a nightmare 
trying to get the plugins set up. Unity needs all the sqlite plugins
to be in a certain directory structure in order to make things work 
on 64 bit, 32 bit, Unity Editor, and Unity Standalone player. 
Also, the System.data usually isn't included in the build (not sure
why) so if we delete that from DLLs we can't use basic System.data
methods to help deal with the database!
