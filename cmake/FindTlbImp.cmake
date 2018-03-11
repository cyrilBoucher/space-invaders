# - Try to find TlbImp executable (Microsoft Type Library Importer)
# Once done this will define
#  TLBIMP_FOUND - System has TlbImp
#  TLBIMP_EXECUTABLE - The absolute path to the TlbImp executable

include(FindPackageHandleStandardArgs)

find_program(TLBIMP_EXECUTABLE
	NAMES TlbImp
	PATHS "C:/Program Files (x86)/Microsoft SDKs/Windows/v8.1A/bin/NETFX 4.5.1 Tools"
		"C:/Program Files (x86)/Microsoft SDKs/Windows/v8.0A/bin/NETFX 4.0 Tools"
		"C:/Program Files (x86)/Microsoft SDKs/Windows/v7.0A/Bin"
)

find_package_handle_standard_args(TlbImp DEFAULT_MSG
                                  TLBIMP_EXECUTABLE
)

mark_as_advanced(TLBIMP_EXECUTABLE)

set(TLBIMP_EXECUTABLE ${TLBIMP_EXECUTABLE})