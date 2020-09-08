@echo on

REM 빌드 환경설정 Script 실행
@call "%VS140COMNTOOLS%vsvars32.bat"

REM 빌드 실행
@devenv "../Hicom.BizDraw.sln" /clean "Debug|Any CPU"
@devenv "../Hicom.BizDraw.sln" /rebuild "Debug|Any CPU"

:end