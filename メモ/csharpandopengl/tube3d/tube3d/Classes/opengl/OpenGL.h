#if !defined(AFX_OPENGL_H__EA152DEA_9FDA_43DF_9F97_9B195FD0C217__INCLUDED_)
#define AFX_OPENGL_H__EA152DEA_9FDA_43DF_9F97_9B195FD0C217__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// OpenGL.h : �w�b�_�[ �t�@�C��
//

/////////////////////////////////////////////////////////////////////////////
// COpenGL �E�B���h�E

class COpenGL : public CObject
{
// �R���X�g���N�V����
public:
	COpenGL();

// �A�g���r���[�g
public:
	HDC hdc;
	HGLRC hglrc;
	CPalette    m_cPalette;
	CPalette    *m_pOldPalette;
	CClientDC   *m_pDC;
	int mousestatus; /*0: normal 1:LbuttonDown 2:Dragging 3:LbuttonUp*/
	CPoint posDown, posDrag, posUp;
	int tick;
	double leye; double dip; double rot; //dip,rot[rad]


// �I�y���[�V����
public:
	//BOOL attachWindow(int nID, CWnd* pParentWnd);
	unsigned char ComponentFromIndex(int i, UINT nbits, UINT shift);
	void CreateRGBPalette();
	void initializeScene(void);
	void paintWindow(void);
	void renderScene(void);
	void PreSubclassWindow();

// �I�[�o�[���C�h
	// ClassWizard �͉��z�֐��̃I�[�o�[���C�h�𐶐����܂��B

	//{{AFX_VIRTUAL(COpenGL)
	//protected:
	//virtual void PreSubclassWindow();
	//}}AFX_VIRTUAL

// �C���v�������e�[�V����
public:
	virtual ~COpenGL();

	// �������ꂽ���b�Z�[�W �}�b�v�֐�
protected:
	//{{AFX_MSG(COpenGL)
	//afx_msg void OnDestroy();
	//afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	//afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	//afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	//afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	//DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ �͑O�s�̒��O�ɒǉ��̐錾��}�����܂��B

#endif // !defined(AFX_OPENGL_H__EA152DEA_9FDA_43DF_9F97_9B195FD0C217__INCLUDED_)
