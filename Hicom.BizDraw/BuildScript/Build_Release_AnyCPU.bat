@echo on

REM ���� ȯ�漳�� Script ����
@call "%VS140COMNTOOLS%vsvars32.bat"

REM ���� ����
@devenv "../Hicom.BizDraw.sln" /clean "Release|Any CPU"
@devenv "../Hicom.BizDraw.sln" /rebuild "Release|Any CPU"

:end