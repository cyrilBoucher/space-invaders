# - Try to find wmp.dll (Windows Media Player library)
# Once done this will define
#  WMP_FOUND - System has wmp library
#  WMP_LIBRARY - The absolute path to the wmp library

include(FindPackageHandleStandardArgs)

# using find_file in place of find_library because
# calls to find_library will be failing on Windows as
# CMAKE_FIND_LIBRARY_SUFFIXES is set to '.lib' only
find_file(WMP_LIBRARY
	NAMES wmp.dll
	PATHS "C:/Windows/System32"
	NO_DEFAULT_PATH
)

find_package_handle_standard_args(wmp DEFAULT_MSG
                                  WMP_LIBRARY
)

mark_as_advanced(WMP_LIBRARY)

set(WMP_LIBRARY ${WMP_LIBRARY})