﻿using System;

namespace MatrixSDK.Backends
{
	public enum MatrixErrorCode{
		M_FORBIDDEN,
		M_UNKNOWN_TOKEN,
		M_BAD_JSON,
		M_NOT_JSON,
		M_NOT_FOUND,
		M_LIMIT_EXCEEDED,
		M_USER_IN_USE,
		M_ROOM_IN_USE,
		M_BAD_PAGINATION,
		M_EXCLUSIVE,
		M_UNKNOWN,
		M_TOO_LARGE,
		CL_UNKNOWN_ERROR_CODE,
		CL_NONE
	}
}

