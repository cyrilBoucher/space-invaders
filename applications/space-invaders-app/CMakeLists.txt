add_executable(space-invaders-app Program.cs)

target_link_libraries(space-invaders-app PUBLIC
	SpaceInvaders
)

set_target_properties(space-invaders-app
	PROPERTIES VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.0"
	VS_DOTNET_REFERENCES "System;System.Windows.Forms"
)

add_custom_command(TARGET space-invaders-app
    POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_directory ${PROJECT_SOURCE_DIR}/space-invaders/Resources $<TARGET_FILE_DIR:space-invaders-app>/Resources
    COMMENT "Copying resources"
)