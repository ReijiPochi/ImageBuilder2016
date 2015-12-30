#pragma once

// CTube3DMain �R�}���h �^�[�Q�b�g

class CTube3DMain : public CObject
{
private:
	float*	m_PointDataX;					//���_�Q�f�[�^
	float*	m_PointDataY;
	float*	m_PointDataZ;
	int		m_PointDataCount;

	float*	m_PointDataFrameX;				//�P�t���[���p�_�Q�f�[�^
	float*	m_PointDataFrameY;
	float*	m_PointDataFrameZ;
	int		m_PointDataFrameCount;

	HWND m_WindowHandle;					//�`����s���R���g���[���̃n���h��

	HDC m_hDC;								//GDC
	HGLRC m_hRC;							//OpenGL�̃R���e�L�X�g

	float  m_ThetaX;						//�V�[�^X����]
	float  m_ThetaY;						//�V�[�^Y����]		

	CPalette    m_cPalette;
	HPALETTE m_pOldPalette;

	int mousestatus; /*0: normal 1:LbuttonDown 2:Dragging 3:LbuttonUp*/
	CPoint posDown, posDrag, posUp;
	double leye; double dip; double rot; //dip,rot[rad]
	int tick;


	void InitOpenGL();
	void DestroyOpenGL();
	void initializeScene();
	void CreateRGBPalette();
	unsigned char ComponentFromIndex(int i, UINT nbits, UINT shift);
	void renderScene(void);

public:
	CTube3DMain();
	virtual ~CTube3DMain();

	int Open();
	int Close();

	int DrawData();				

	int SetMousePosUp(int x, int y);				
	int SetMousePosDown(int x, int y);				
	int SetMousePosMove(int x, int y);				

	int set_WindowHandle(HANDLE h);					//�E�C���h�E�n���h���̃Z�b�g

};


