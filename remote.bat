@echo off

set /p rclonePath=<remote\rclonePath.txt
set rpath="%cd%%rclonePath%"
set crpath=%cd%%rclonePath%
set rcloneArgs=

if "%~1"=="" (goto help)
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

if %1==-rc (goto rcloneCmd)

:help
    type remote\help.txt
    goto eof

:lpath
    echo %rpath%
    goto eof

:pull
    if "%~2"=="" (
        remote\rclone copy Remote: %rpath% -u
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy Remote: %rpath% -u %rcloneArgs%
    goto epf

:get
    if "%~2"="" (
        remote\rclone copy Remote: %rpath%
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy Remote: %rpath%
    goto eof

:push
    if "%~2"=="" (
        remote\rclone copy %rpath% Remote: -u
        goto eof
    )
    if %2==-f (
        goto pushfile
    )
    if %2==-d (
        goto pushdir
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy %rpath% Remote: -u %rcloneArgs%
    goto eof

:pushfile
    if "%~3"=="" (
        goto help
    )
    setlocal EnableDelayedExpansion
    set filePath=%3
        set filePath=!filePath:%crpath%=!
        set filePath=!filePath:"=!
        set args=%*
        set args=!args:%3%=!
        for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
        remote\rclone copy !rpath! Remote: --include "!filePath:\=/!" -u !rcloneArgs!
    setlocal DisableDelayedExpansion
    goto eof

:pushdir
    if "%~3"=="" (
        goto help
    )
    setlocal EnableDelayedExpansion
    set filePath=%3
        set filePath=!filePath:%crpath%=!
        set filePath=!filePath:"=!
        set args=%*
        set args=!args:%3%=!
        for /f "tokens=2,* delims= " %%a in ("!args!") do set rcloneArgs=%%b
        remote\rclone copy !rpath! Remote: --include "!filePath:\=/!*" -u !rcloneArgs!
    setlocal DisableDelayedExpansion
    goto eof

:put
    if "%~2"=="" (
        remote\rclone copy %rpath% Remote:
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone copy %rpath% Remote: %rcloneArgs%
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
            remote\rclone sync Remote: %rpath%
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
                remote\rclone sync %rpath% Remote: %rcloneArgs%
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
        remote\rclone sync Remote: %rpath% %rcloneArgs%
    )
    goto eof

:rebasefile
    if "%~3"=="" (
        goto help
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
        remote\rclone sync Remote: !rpath! --include "!filePath:\=/!" !rcloneArgs!
        setlocal DisableDelayedExpansion
    )
    goto eof

:rebasedir
    if "%~3"=="" (
        goto help
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
    remote\rclone sync !rpath! Remote: --include "!filePath:\=/!*" !rcloneArgs!
    setlocal DisableDelayedExpansion
    )
    goto eof

:rebaseremotefile
    if "%~3"=="" (
        goto help
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
    remote\rclone sync Remote: !rpath! --include "!filePath:\=/!" !rcloneArgs!
    setlocal DisableDelayedExpansion
    )
    goto eof

:rebaseremotedir
    if "%~3"=="" (
        goto help
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
    remote\rclone sync Remote: !rpath! --include "!filePath:\=/!*" !rcloneArgs!
    setlocal DisableDelayedExpansion
    )
    goto eof

:list
    if "%~2"=="" (
        remote\rclone tree Remote: --exclude *.meta
        goto eof
    )
    if %2==-im (
        for /f "tokens=2,* delims= " %%a in ("%*") do set rcloneArgs=%%b
        remote\rclone tree Remote: %rcloneArgs%
        goto eof
    )
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    echo remote\rclone tree Remote: --exclude *.meta %rcloneArgs%
    remote\rclone tree Remote: --exclude *.meta %rcloneArgs%
    goto eof

:find
    if "%~2"=="" (
        goto help
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
    remote\rclone ls Remote: --include "!filename!" !rcloneArgs!
    goto eof
    setlocal DisableDelayedExpansion
goto eof

:status
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone check %rpath% Remote: %rcloneArgs%
    goto eof

:quota
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone about Remote: %rcloneArgs%
    goto eof

:rcloneCmd
    for /f "tokens=1,* delims= " %%a in ("%*") do set rcloneArgs=%%b
    remote\rclone %rcloneArgs%
    goto eof

:eof
