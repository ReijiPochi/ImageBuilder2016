// tube3d.h : tube3d.DLL �̃��C�� �w�b�_�[ �t�@�C��
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH �ɑ΂��Ă��̃t�@�C�����C���N���[�h����O�� 'stdafx.h' ���C���N���[�h���Ă�������"
#endif

#include "resource.h"		// ���C�� �V���{��


// Ctube3dApp
// ���̃N���X�̎����Ɋւ��Ă� tube3d.cpp ���Q�Ƃ��Ă��������B
//

class Ctube3dApp : public CWinApp
{
public:
	Ctube3dApp();

// �I�[�o�[���C�h
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
