@echo off

set /p rclonePath=<remote\rclonePath.txt
set rpath="%cd%%rclonePath%"
set crpath=%cd%%rclonePath%
set excludepath=%cd%/remote/rcloneExclude.txt
set versionpath=%cd%/remote/version.txt
set versionCheckPath=%cd%/remote/versionCheck.txt
set rcloneArgs=

if "%~1"=="" (goto commanderror)
if %1==help (goto help)
if %1==path (goto lpath)

if %1==pull (goto pull)
if %1==get (goto get)
if %1==push (goto push)
if %1==put (goto put)
if %1==status (goto status)
if %1==rebase (goto rebase)
if %1==list (goto list)
if %1==find (goto find)
if %1==quota (goto quota)
if %1==backup (goto backup)
if %1==stash (goto stash)
if %1==version (goto version)

if %1==-rc (goto rcloneCmd)

:commanderror
    echo Command not recognised or used wrong!
    echo use "help" for list of commands
    goto eof

:help
    type remote\help.txt
    goto eof

:lpath
    echo %rpath%
    goto eof

:copy
    if "%~3"=="" (
        robocopy "%~1" "%~2" /E /V /R:10 /W:30 /ETA
    ) else (
        robocopy "%~1" "%~2" /E /V /LOG:"%~3" /R:10 /W:30 /ETA
    )
    
    exit /b 0

:copyfile
    if "%~4"=="" (
        robocopy "%~1" "%~2" "%~3" /R:10 /W:30 /ETA
    ) else (
        robocopy "%~1" "%~2" "%~3" /LOG:"%~4" /R:10 /W:30 /ETA
    )
    exit /b 0

:replace
    robocopy "%~1" "%~2" /E /V /XF "%~3" /R:10 /W:30 /ETA /MIR
    exit /b 0


:backup
    if "%~2"=="-apply" (
        goto backupapply
    )
    if "%~2"=="-list" (
        goto backuplist
    )
    if "%~2"=="-restore" (
        goto backuprestore
    )
    if "%~2"=="-clear" (
        goto backupclear
    )
    if not "%~2"=="" (
        goto commanderror
    )
    for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
    set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
    set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
    set "datestamp=%YYYY%-%MM%-%DD%" & set "timestamp=%HH%-%Min%-%Sec%"
    set backupname=
    set /p "backupname=Name backup (leave blank for backup_date_time):"
    if "%backupname%"=="" (set backupname=backup_%datestamp%_%timestamp%)
    mkdir "%cd%\backup\%backupname%"
    call :copy "%cd%\qUp\Assets\Media" "%cd%\backup\%backupname%\Media" "%cd%\backup\%backupname%\BackupInfo.txt"
    call :copyfile "%cd%\qUp\Assets" "%cd%\backup\%backupname%" Media.meta "%cd%\backup\%backupname%\BackupInfo.txt"
    goto eof

:backupapply
    if "%~3"=="" (
        goto commanderror
    )
    call :confirm "Apply %~3 (all changes made after this backup will be lost)?",confirmed
    if "%confirmed%"=="0" (
        goto eof
    )
    if exist "%cd%\backup\%~3" (
        call :replace "%cd%\backup\%~3\Media" "%cd%\qUp\Assets\Media" BackupInfo.txt
        call :copyfile "%cd%\backup\%~3" "%cd%\qUp\Assets" Media.meta
        goto eof
    ) else (
        echo There is no such backup!
        echo Use command "backup list" to view available backups
        goto eof
    )
    goto eof

:backuplist
    set "_TMP="
    for /f "delims=" %%a in ('dir /b %cd%\backup') do if not "%%a"=="stash" (set "_TMP=%%a")
    if "%_TMP%"=="" (
        echo No backups.
        echo Make your first backup with "backup" command.
        goto eof
    )
    echo Available backups:
    echo.
    dir /b /ad %cd%\backup /O:-D | findstr /v "stash"
    echo.
    goto eof

:backuprestore
    set "latest="
    FOR /F "delims=" %%i IN ('dir %cd%\backup /b /ad-h /t:c /od') DO if not "%%i"=="stash" (SET "latest=%%i")
    if "%latest%"=="" (
        echo No backup available.
        echo Make your first backup with "backup" command.
        goto eof
    )
    call :confirm "Apply '%latest%' backup (all changes made after this backup will be lost)?",confirmed
    if "%confirmed%"=="0" (
        goto eof
    )
    call :replace "%cd%\backup\%latest%\Media" "%cd%\qUp\Assets\Media" BackupInfo.txt
    call :copyfile "%cd%\backup\%latest%" "%cd%\qUp\Assets" Media.meta
    goto eof

:backupclear
    if "%~3"=="-all" (
        goto backupclearall
    )
    if not "%~3"=="" (
        goto commanderror
    )
    call :confirm "This will remove all but latest backup. Are you sure you want to proceed?",confirmed
    if "%confirmed%"=="0" (
        goto eof
    )
    set "latest="
    FOR /F "delims=" %%i IN ('dir %cd%\backup /b /ad-h /t:c /od') DO if not "%%i"=="stash" (SET "latest=%%i")
    if "%latest%"=="" (
        echo No backup to clear.
        echo Make your first backup with "backup" command.
        goto eof
    )
    for /f "delims=" %%a in ('dir /b %cd%\backup') do if not "%%a"=="stash" (
        if not "%%a"=="%latest%" (
            del /f /s /q "%cd%\backup\%%a" 1>nul
            rmdir /s /q "%cd%\backup\%%a" 1>nul
        )
    )
    goto eof

:backupclearall
    call :confirm "This will remove all backups. Are you sure you want to proceed?",confirmed
    if "%confirmed%"=="0" (
        goto eof
    )
    for /f "delims=" %%a in ('dir /b %cd%\backup') do if not "%%a"=="stash" (
        del /f /s /q "%cd%\backup\%%a" 1>nul
        rmdir /s /q "%cd%\backup\%%a" 1>nul
    )
    goto eof

:stash
    if "%~2"=="-m" (
        goto stashmeta
    )
    if "%~2"=="-status" (
        goto stashstatus
        goto eof
    )
    if "%~2"=="-apply" (
        if "%~3"=="-m" (
            goto stashmeta
        )
        goto stashapply
        goto eof
    )
    if "%~2"=="-clear" (
        del /f /s /q "%cd%\backup\stash" 1>nul
        rmdir /s /q "%cd%\backup\stash" 1>nul
        goto eof
    )
    if not "%~2"=="" (
        goto commanderror
    )
    if "%~2"=="" (
        mkdir %cd%\backup\stash\Media
        robocopy "%cd%\qUp\Assets\Media" "%cd%\backup\stash\Media" /XF *.meta /E /V /R:10 /W:30 /ETA /MIR
        goto eof
    )
    goto eof

:stashstatus
    set _TMP=
    if not exist "%cd%\backup\stash" (
        echo Stash is empty.
        echo Stash your files with "stash" command.
        goto eof
    )
    if not exist "%cd%\backup\stash\Media" (
        echo Stash is empty.
        echo Stash your files with "stash" command.
        goto eof
    )
    for /f "delims=" %%a in ('dir /b "%cd%\backup\stash\Media\"') do set _TMP=%%a
    IF "%_TMP%"=="" (
        echo Stash is empty.
        echo Stash your files with "stash" command.
        goto eof
    ) ELSE (
        echo Stash is ready.
        echo Apply stash to working directory with "stash -apply" command.
        goto eof
    )
    goto eof

:stashapply
    if not exist "%cd%\backup\stash" (
        echo Stash is empty.
        echo Stash your files with "stash" command.
        goto eof
    )
    if not exist "%cd%\backup\stash\Media" (
        echo Stash is empty.
        echo Stash your files with "stash" command.
        goto eof
    )
    call :confirm "Apply stash (all changes made after this stash will be lost)?",confirmed
    if "%confirmed%"=="0" (
        goto eof
    )
    set _TMP= 
    for /f "delims=" %%a in ('dir /b "%cd%\backup\stash\Media\"') do set _TMP=%%a
    IF NOT "%_TMP%"=="" (
        robocopy "%cd%\backup\stash\Media" %rpath% /E /V /R:10 /W:30 /ETA /MIR /XF *.meta
    ) else (
        echo Stash is empty.
        echo Stash your files with "stash" command.
        goto eof
    )
    goto eof


:stashmeta
    if "%~2"=="-m" (
        mkdir "%cd%\backup\stash\Media"
        robocopy "%cd%\qUp\Assets\Media" "%cd%\backup\stash\Media" /E /V /R:10 /W:30 /ETA /MIR
        robocopy "%cd%\qUp\Assets" "%cd%\backup\stash" Media.meta
        goto eof
    )
    if "%~2"=="-apply" (
        if "%~3"=="-m" (
            goto stashapplymeta
        )
        goto eof
    )
    goto eof

:stashapplymeta
    if not exist "%cd%\backup\stash" (
        echo Stash is empty.
        echo Stash your files with "stash -m" command.
        goto eof
    )
    if not exist "%cd%\backup\stash\Media" (
        echo Stash is empty.
        echo Stash your files with "stash -m" command.
        goto eof
    )
    call :confirm "Apply stash (all changes made after this stash will be lost)?",confirmed
    if "%confirmed%"=="0" (
        goto eof
    )
    set _TMP= 
    for /f "delims=" %%a in ('dir /b "%cd%\backup\stash\Media\"') do set _TMP=%%a
    IF NOT "%_TMP%"=="" (
        robocopy "%cd%\backup\stash\Media" %rpath% /E /V /R:10 /W:30 /ETA /MIR
        robocopy "%cd%\backup\stash" "%cd%\qUp\Assets" Media.meta
    ) else (
        echo Stash is empty.
        echo Stash your files with "stash -m" command.
        goto eof
    )
    goto eof

:pull
    if "%~2"=="" (
        remote\rclone copy Remote: %rpath% -u --exclude-from %excludepath%
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy Remote: %rpath% -u --exclude-from %excludepath% %rcloneArgs%
    goto epf

:get
    if "%~2"="" (
        remote\rclone copy Remote: %rpath% --exclude-from %excludepath%
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy Remote: %rpath% --exclude-from %excludepath%
    goto eof

:push
    if "%~2"=="" (
        remote\rclone copy %rpath% Remote: -u --exclude-from %excludepath%
        goto eof
    )
    if %2==-f (
        goto pushfile
    )
    if %2==-d (
        goto pushdir
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy %rpath% Remote: -u --exclude-from %excludepath% %rcloneArgs%
    goto eof

:pushfile
    if "%~3"=="" (
        goto commanderror
    )
    setlocal EnableDelayedExpansion
    set filePath=%3
        set filePath=!filePath:%crpath%=!
        set filePath=!filePath:"=!
        set args=%*
        set args=!args:%3%=!
        for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
        remote\rclone copy !rpath! Remote: --exclude-from %excludepath% --include "!filePath:\=/!" -u !rcloneArgs!
    setlocal DisableDelayedExpansion
    goto eof

:pushdir
    if "%~3"=="" (
        goto commanderror
    )
    setlocal EnableDelayedExpansion
    set filePath=%3
        set filePath=!filePath:%crpath%=!
        set filePath=!filePath:"=!
        set args=%*
        set args=!args:%3%=!
        for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
        remote\rclone copy !rpath! Remote: --exclude-from %excludepath% --include "!filePath:\=/!*" -u !rcloneArgs!
    setlocal DisableDelayedExpansion
    goto eof

:put
    if "%~2"=="" (
        remote\rclone copy %rpath% Remote: --exclude-from %excludepath%
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy %rpath% Remote: --exclude-from %excludepath% %rcloneArgs%
    goto eof

:rebase
    set confirm=n
    if "%~2"=="" (
        echo.
        echo Are you sure?
        echo.
        echo This will overide all your changes and delete all
        echo files that you have but dont exist on remote!

        set /p confirm="(y/n)"
        if %confirm%==y (
            remote\rclone sync Remote: %rpath% --exclude-from %excludepath%
        )
        goto eof
    )
    if %2==-!REMOTE (
        if "%~3"=="" (
            echo.
            echo Are you sure?
            echo.
            echo This will overide all newever files on remote
            echo with your files and it will remove all files
            echo that exist on remote but you dont have!

            set /p confirm="(y/n)"

            for /f "tokens=2,* delims= " %%a in ("%*") do set rcloneArgs=%%b
            if %confirm%==y (
                remote\rclone sync %rpath% Remote: --exclude-from %excludepath% %rcloneArgs%
            )
            goto eof
        )
        if %3==-f (
            goto rebaseremotefile
        )
        if %3==-d (
            goto rebaseremotedir
        )
    )
    if %2==-f (
        goto rebasefile
    )
    if %2==-d (
        goto rebasedir
    )
    echo.
    echo Are you sure?
    echo.
    echo This will overide all your changes and delete all
    echo files that you have but dont exist on remote!

    set /p confirm="(y/n)"
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    if %confirm%==y (
        remote\rclone sync Remote: %rpath% --exclude-from %excludepath% %rcloneArgs%
    )
    goto eof

:rebasefile
    if "%~3"=="" (
        goto commanderror
    )
    echo.
    echo Are you sure?
    echo.
    echo This will overide all your changes on a file.
    set /p confirm="(y/n)"

    if %confirm%==y (
        setlocal EnableDelayedExpansion
        set filePath=%3
        set filePath=!filePath:%crpath%=!
        set filePath=!filePath:"=!
        set args=%*
        set args=!args:%3%=!
        for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
        remote\rclone sync Remote: !rpath! --exclude-from %excludepath% --include "!filePath:\=/!" !rcloneArgs!
        setlocal DisableDelayedExpansion
    )
    goto eof

:rebasedir
    if "%~3"=="" (
        goto commanderror
    )
    echo.
    echo Are you sure?
    echo.
    echo This will overide all your changes in a directory.
    set /p confirm="(y/n)"

    if %confirm%==y (
    setlocal EnableDelayedExpansion
    set filePath=%3
    set filePath=!filePath:%crpath%=!
    set filePath=!filePath:"=!
    set args=%*
    set args=!args:%3%=!
    for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
    remote\rclone sync !rpath! Remote: --exclude-from %excludepath% --include "!filePath:\=/!*" !rcloneArgs!
    setlocal DisableDelayedExpansion
    )
    goto eof

:rebaseremotefile
    if "%~3"=="" (
        goto commanderror
    )
    echo.
    echo Are you sure?
    echo.
    echo This will overide all changes on remote with you file.
    set /p confirm="(y/n)"

    if %confirm%==y (
    setlocal EnableDelayedExpansion
    set filePath=%3
    set filePath=!filePath:%crpath%=!
    set filePath=!filePath:"=!
    set args=%*
    set args=!args:%3%=!
    for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
    remote\rclone sync Remote: !rpath! --exclude-from %excludepath% --include "!filePath:\=/!" !rcloneArgs!
    setlocal DisableDelayedExpansion
    )
    goto eof

:rebaseremotedir
    if "%~3"=="" (
        goto commanderror
    )
    echo.
    echo Are you sure?
    echo.
    echo This will overide all changes on remote with you directory.
    set /p confirm="(y/n)"

    if %confirm%==y (
    setlocal EnableDelayedExpansion
    set filePath=%3
    set filePath=!filePath:%crpath%=!
    set filePath=!filePath:"=!
    set args=%*
    set args=!args:%3%=!
    for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
    remote\rclone sync Remote: !rpath! --exclude-from %excludepath% --include "!filePath:\=/!*" !rcloneArgs!
    setlocal DisableDelayedExpansion
    )
    goto eof

:list
    if "%~2"=="" (
        remote\rclone tree Remote: --exclude *.meta --exclude-from %excludepath%
        goto eof
    )
    if %2==-im (
        for /f "tokens=2,* delims= " %%a in ("%*") do set rcloneArgs=%%b
        remote\rclone tree Remote: --exclude-from %excludepath% %rcloneArgs% 
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    echo remote\rclone tree Remote: --exclude *.meta %rcloneArgs%
    remote\rclone tree Remote: --exclude-from %excludepath% --exclude *.meta %rcloneArgs%
    goto eof

:find
    if "%~2"=="" (
        goto commanderror
    )
    setlocal EnableDelayedExpansion
    set filename=%2
    if "%~x2"=="" (
        set filename=%2*
    )
    set filename=!filename:"=!
    set args=%*
    set args=!args:%2%=!
    for /f "tokens=1,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
    remote\rclone ls Remote: --exclude-from %excludepath% --include "!filename!" !rcloneArgs!
    goto eof
    setlocal DisableDelayedExpansion
goto eof

:status
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone check %rpath% Remote: --exclude-from %excludepath% %rcloneArgs%
    goto eof

:quota
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone about Remote: --exclude-from %excludepath% %rcloneArgs%
    goto eof

:rcloneCmd
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone %rcloneArgs%
    goto eof

:confirm
    set confirm=n
    set /p confirm="%~1(y/n)"
    if NOT "%confirm%"=="y" (
        echo CANCELED
        set "%~2=0"
        exit /b 0
    )
    set "%~2=1"
    exit /b 0

:version
    if "%~2"=="-releaseVersion" (
        goto releaseVersion
    )
    echo Remote
    type remote\version.txt
    echo.
    break>%versionCheckPath%
    remote\rclone check %versionpath% RemoteConfig: --quiet --log-file %versionCheckPath%
    for %%a in ("%versionCheckPath%") do if %%~za==0 (
        echo You have the newest version!
    ) else (
        echo New version is available! Pull git master to update.
    )
    goto eof

:releaseVersion
    remote\rclone sync %versionpath% RemoteConfig: -q
    echo New version published!

:eof
