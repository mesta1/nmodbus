@echo off
if not exist output mkdir output
cd output
"C:\Program Files\Sandcastle\ProductionTools\MRefBuilder" "..\..\src\Modbus\bin\Release\Modbus.dll" /out:reflection.org
copy "..\..\src\Modbus\bin\Release\Modbus.xml" comments.xml
"C:\Program Files\Sandcastle\ProductionTools\XslTransform" "C:\Program Files\Sandcastle\ProductionTransforms\AddOverloads.xsl" reflection.org | "C:\Program Files\Sandcastle\ProductionTools\XslTransform" "C:\Program Files\Sandcastle\ProductionTransforms\AddGuidFilenames.xsl" /out:reflection.xml
"C:\Program Files\Sandcastle\ProductionTools\XslTransform" "C:\Program Files\Sandcastle\ProductionTransforms\ReflectionToManifest.xsl"  reflection.xml /out:manifest.xml
if not exist html mkdir html
if not exist art mkdir art
if not exist scripts mkdir scripts
if not exist styles mkdir styles
copy "C:\Program Files\Sandcastle\Presentation\art\*" art
copy "C:\Program Files\Sandcastle\Presentation\scripts\*" scripts
copy "C:\Program Files\Sandcastle\Presentation\styles\*" styles
"C:\Program Files\Sandcastle\ProductionTools\BuildAssembler" /config:../sandcastle.config manifest.xml
"C:\Program Files\Sandcastle\ProductionTools\XslTransform" "C:\Program Files\Sandcastle\ProductionTransforms\ReflectionToChmContents.xsl" reflection.xml /arg:html="html" /out:"Test.hhc"
if not exist help_proj.hhp copy "C:\Program Files\Sandcastle\Presentation\Chm\test.hhp" help_proj.hhp
"C:\Program Files\HTML Help Workshop\hhc.exe" "%CD%\help_proj.hhp"
@copy "Test.chm" "..\NModbus.chm"
@cd ..
@rd /s /q output