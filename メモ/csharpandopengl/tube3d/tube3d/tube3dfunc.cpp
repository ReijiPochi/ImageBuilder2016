#include "StdAfx.h"

#include "tube3dFunc.h"

#include ".\Classes\Job\Tube3DMain.h"
///--------------------------------------------------------------------
///�萔��`
///
///--------------------------------------------------------------------

///--------------------------------------------------------------------
///�ϐ���`
///
///--------------------------------------------------------------------

// Enable OpenGL





//HWND g_WindowHandle;

//HDC g_hDC;
//HGLRC g_hRC;


CTube3DMain * m_MainObj = NULL;

///--------------------------------------------------------------------
///�e�C���X�^���X�𐶐�
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
///�e�C���X�^���X��j��
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
///�I�[�v��
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
///�N���[�Y
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
///3�����`����s��
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
///�}�E�X�̍��W��^����(Mouse Up)
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
///�}�E�X�̍��W��^����(Mouse Down)
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
///�}�E�X�̍��W��^����(Mouse Move)
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
///�E�C���h�E�n���h���̃Z�b�g
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

