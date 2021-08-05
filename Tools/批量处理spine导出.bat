for /r %%i in (*.atlas) do ( 
echo %%i
ren "%%i" "%%~ni.atlas.txt"
 )
 for /r %%i in (*.skel) do ( 
echo %%i
ren "%%i" "%%~ni.skel.bytes"
 )
pause
exit