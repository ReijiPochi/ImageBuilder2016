#include "StdAfx.h"

#include "tube3dFunc.h"

#include ".\Classes\Job\Tube3DMain.h"
///--------------------------------------------------------------------
///定数定義
///
///--------------------------------------------------------------------

///--------------------------------------------------------------------
///変数定義
///
///--------------------------------------------------------------------

// Enable OpenGL





//HWND g_WindowHandle;

//HDC g_hDC;
//HGLRC g_hRC;


CTube3DMain * m_MainObj = NULL;

///--------------------------------------------------------------------
///各インスタンスを生成
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_Create()
{
	if (m_MainObj != NULL)
	{
		return ERRORCODE_TUBE3D_DOUBLECREATE;
	}

	m_MainObj = new CTube3DMain();

	return ERRORCODE_TUBE3D_NON;
}

///--------------------------------------------------------------------
///各インスタンスを破棄
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_Destroy()
{
	if (m_MainObj != NULL)
	{
		delete m_MainObj;
		m_MainObj = NULL;
	}

	return ERRORCODE_TUBE3D_NON;
}

///--------------------------------------------------------------------
///オープン
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_Open()										
{
	if (m_MainObj == NULL)
	{
		return ERRORCODE_TUBE3D_NOTCREATE;
	}
	return m_MainObj->Open();
}

///--------------------------------------------------------------------
///クローズ
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_Close()										
{
	if (m_MainObj == NULL)
	{
		return ERRORCODE_TUBE3D_NOTCREATE;
	}

	return m_MainObj->Close();
}


///--------------------------------------------------------------------
///3次元描画を行う
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_DrawData()
{
	if (m_MainObj == NULL)
	{
		return ERRORCODE_TUBE3D_NOTCREATE;
	}

	return m_MainObj->DrawData();
}

///--------------------------------------------------------------------
///マウスの座標を与える(Mouse Up)
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_SetMousePosUp(int x, int y)
{
	if (m_MainObj == NULL)
	{
		return ERRORCODE_TUBE3D_NOTCREATE;
	}

	return m_MainObj->SetMousePosUp(x, y);
}

///--------------------------------------------------------------------
///マウスの座標を与える(Mouse Down)
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_SetMousePosDown(int x, int y)
{
	if (m_MainObj == NULL)
	{
		return ERRORCODE_TUBE3D_NOTCREATE;
	}
	return m_MainObj->SetMousePosDown(x, y);
}

///--------------------------------------------------------------------
///マウスの座標を与える(Mouse Move)
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_SetMousePosMove(int x, int y)
{
	if (m_MainObj == NULL)
	{
		return ERRORCODE_TUBE3D_NOTCREATE;
	}
	return m_MainObj->SetMousePosMove(x, y);
}


///--------------------------------------------------------------------
///ウインドウハンドルのセット
///
///--------------------------------------------------------------------
extern "C" int WINAPI TUBE3D_set_WindowHandle(HANDLE h)					
{
	if (m_MainObj == NULL)
	{
		return ERRORCODE_TUBE3D_NOTCREATE;
	}

	return m_MainObj->set_WindowHandle(h);
}

