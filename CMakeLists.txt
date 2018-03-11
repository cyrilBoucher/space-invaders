cmake_minimum_required(VERSION 3.8)

# for some reason this is the only I managed to make the langversion
# match the target version further down
set(CMAKE_CSharp_FLAGS "/platform:x64 /define:TRACE /langversion:4" CACHE STRING "Flags used by the C# compiler")

project(SpaceInvaders
	LANGUAGES CSharp
	VERSION 1.0.0
)

set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} ${CMAKE_CURRENT_SOURCE_DIR}/cmake)

set(CMAKE_DEBUG_POSTFIX "d")

find_package(wmp REQUIRED)
find_package(TlbImp REQUIRED)

include(CSharpUtilities)

set(_outLib ${CMAKE_CURRENT_BINARY_DIR}/generated/Interop.WMPLib.dll)
execute_process(COMMAND ${TLBIMP_EXECUTABLE} ${WMP_LIBRARY} /namespace:WMPLib /out:${_outLib}
	RESULT_VARIABLE tlbimp_result
	ERROR_FILE ${CMAKE_CURRENT_BINARY_DIR}/tlbimp_log.txt
)

set(_srcs
	space-invaders/Bonus.cs
	space-invaders/Bunker.cs
	space-invaders/EnemyBlock.cs
	space-invaders/FallingBall.cs
	space-invaders/Game.cs
	space-invaders/Intro.cs
	space-invaders/Lost.cs
	space-invaders/MainMenu.cs
	space-invaders/Missile.cs
	space-invaders/Score.cs
	space-invaders/SpaceShip.cs
	space-invaders/Vecteur2D.cs
	space-invaders/Win.cs
	space-invaders/Properties/AssemblyInfo.cs
	space-invaders/Properties/Resources.resx
	space-invaders/Properties/Resources.Designer.cs
	space-invaders/Properties/Settings.settings
	space-invaders/Properties/Settings.Designer.cs
	space-invaders/Form1.cs
	space-invaders/Form1.Designer.cs
	space-invaders/Form1.resx
)

set(_rsrcs
	space-invaders/Resources/BigPew.wav
	space-invaders/Resources/bonus.png
	space-invaders/Resources/bonus2.png
	space-invaders/Resources/bonus3.png
	space-invaders/Resources/Boom.wav
	space-invaders/Resources/Bop.wav
	space-invaders/Resources/bunker.png
	space-invaders/Resources/ding.wav
	space-invaders/Resources/DoublePoints.wav
	space-invaders/Resources/Down.wav
	space-invaders/Resources/Easy.wav
	space-invaders/Resources/EasyMusic.mp3
	space-invaders/Resources/esieelogo.png
	space-invaders/Resources/ethnocentric_rg.ttf
	space-invaders/Resources/Hard.wav
	space-invaders/Resources/HardMusic.mp3
	space-invaders/Resources/IntroMusic.mp3
	space-invaders/Resources/level1Easy.txt
	space-invaders/Resources/level1EasyBackup.txt
	space-invaders/Resources/Level1Hard.txt
	space-invaders/Resources/Level1HardBackup.txt
	space-invaders/Resources/level1Medium.txt
	space-invaders/Resources/level1MediumBackup.txt
	space-invaders/Resources/lost.jpg
	space-invaders/Resources/LostMusic.wav
	space-invaders/Resources/Medium.wav
	space-invaders/Resources/MediumMusic.mp3
	space-invaders/Resources/pause.jpg
	space-invaders/Resources/Pew.wav
	space-invaders/Resources/ship1.png
	space-invaders/Resources/ship2.png
	space-invaders/Resources/ship3.png
	space-invaders/Resources/ship4.png
	space-invaders/Resources/ship5.png
	space-invaders/Resources/ship6.png
	space-invaders/Resources/ship7.png
	space-invaders/Resources/ship8.png
	space-invaders/Resources/ship9.png
	space-invaders/Resources/shoot1.png
	space-invaders/Resources/shoot2.png
	space-invaders/Resources/shoot3.png
	space-invaders/Resources/shoot4.png
	space-invaders/Resources/Suck.wav
	space-invaders/Resources/Touched.wav
	space-invaders/Resources/Up.wav
	space-invaders/Resources/win.jpg
	space-invaders/Resources/WinMusic.wav
)

csharp_set_windows_forms_properties(space-invaders/Form1.cs space-invaders/Form1.Designer.cs space-invaders/Form1.resx)
csharp_set_designer_cs_properties(space-invaders/Properties/Resources.Designer.cs space-invaders/Properties/Resources.resx)
csharp_set_designer_cs_properties(space-invaders/Properties/Settings.Designer.cs space-invaders/Properties/Settings.settings)

add_library(SpaceInvaders ${_srcs} ${_rsrcs})

set_target_properties(SpaceInvaders
	PROPERTIES VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.0"
	VS_DOTNET_REFERENCES "System;System.Data;System.Drawing;System.Windows.Forms;System.Xaml;WMPLib"
	VS_DOTNET_REFERENCEPROP_WMPLib_TAG_HintPath "${CMAKE_CURRENT_BINARY_DIR}/generated/Interop.WMPLib.dll"
)

add_subdirectory(applications)
