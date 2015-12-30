//**Kosaka added this CLASS**
//クラスの新規作成でCWndを基本クラスにして作成
//プロジェクトの設定／リンク／ライブラリモジュールにopengl32.lib glu32.lib 

// OpenGL.cpp : インプリメンテーション ファイル
//

#include "stdafx.h"
//#include "DialogOpenGLtemplate.h"
#include "OpenGL.h"
#include <gl/glu.h> //**Kosaka added this line.**
#include <math.h>
#include "GLDrawingtool3D.h"
#ifndef M_PI
#define M_PI 3.141592653589793
#endif

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// COpenGL

COpenGL::COpenGL()
{
	mousestatus=0;
	leye=700.0;
	dip=0.65;
	rot=0.5; //dip,rot[rad]
}

COpenGL::~COpenGL()
{
}

//BEGIN_MESSAGE_MAP(COpenGL, CWnd)
//	//{{AFX_MSG_MAP(COpenGL)
//	ON_WM_DESTROY()
//	ON_WM_LBUTTONDOWN()
//	ON_WM_LBUTTONUP()
//	ON_WM_MOUSEMOVE()
//	ON_WM_TIMER()
//	//}}AFX_MSG_MAP
//END_MESSAGE_MAP()

//int setPixelFormat(HDC hdc)
//{
//	PIXELFORMATDESCRIPTOR pfd = { 
//	    sizeof(PIXELFORMATDESCRIPTOR),    // size of this pfd 
//	    1,                                // version number 
//	    PFD_DRAW_TO_WINDOW |              // support window 
//	    PFD_SUPPORT_OPENGL |              // support OpenGL 
//	    PFD_DOUBLEBUFFER,                 // double buffered 
//	    PFD_TYPE_RGBA,                    // RGBA type 
//	    24,                               // 24-bit color depth 
//	    0, 0, 0, 0, 0, 0,                 // color bits ignored 
//	    0,                                // no alpha buffer 
//	    0,                                // shift bit ignored 
//	    0,                                // no accumulation buffer 
//	    0, 0, 0, 0,                       // accum bits ignored 
//	    32,                               // 32-bit z-buffer     
//	    0,                                // no stencil buffer 
//	    0,                                // no auxiliary buffer 
//	    PFD_MAIN_PLANE,                   // main layer 
//	    0,                                // reserved 
//	    0, 0, 0                           // layer masks ignored 
//	}; 
//	int  iPixelFormat; 
// 
//	// get the device context's best, available pixel format match 
//	if((iPixelFormat = ChoosePixelFormat(hdc, &pfd)) == 0) {
//		//MessageBox(NULL, "ChoosePixelFormat Failed", NULL, MB_OK);
//		return 0;
//	}
//	 
//	// make that match the device context's current pixel format 
//	if(SetPixelFormat(hdc, iPixelFormat, &pfd) == FALSE) {
//		//MessageBox(NULL, "SetPixelFormat Failed", NULL, MB_OK);
//		return 0;
//	}
//
//	return 1;
//}

//BOOL COpenGL::attachWindow(int nID, CWnd* pParentWnd)
//{
//	TRACE("COpenGL::attachWindow\n");
//
//	return(SubclassDlgItem(nID, pParentWnd));
//}

unsigned char COpenGL::ComponentFromIndex(int i, UINT nbits, UINT shift)
{
	unsigned char threeto8[8] = {
		0, 0111>>1, 0222>>1, 0333>>1, 0444>>1, 0555>>1, 0666>>1, 0377
	};
	unsigned char twoto8[4] = {
		0, 0x55, 0xaa, 0xff
	};
	unsigned char oneto8[2] = {
		0, 255
	};
	unsigned char val = (unsigned char) (i >> shift);
	switch (nbits) {
	case 1:
		val &= 0x1;
		return oneto8[val];
	case 2:
		val &= 0x3;
		return twoto8[val];
	case 3:
		val &= 0x7;
		return threeto8[val];
	default:
		return 0;
	}
}

void COpenGL::CreateRGBPalette()
{
	static int defaultOverride[13] = {
		0, 3, 24, 27, 64, 67, 88, 173, 181, 236, 247, 164, 91
	};
	static PALETTEENTRY defaultPalEntry[20] = {
		{ 0,   0,   0,    0 },
		{ 0x80,0,   0,    0 },
		{ 0,   0x80,0,    0 },
		{ 0x80,0x80,0,    0 },
		{ 0,   0,   0x80, 0 },
		{ 0x80,0,   0x80, 0 },
		{ 0,   0x80,0x80, 0 },
		{ 0xC0,0xC0,0xC0, 0 },

		{ 192, 220, 192,  0 },
		{ 166, 202, 240,  0 },
		{ 255, 251, 240,  0 },
		{ 160, 160, 164,  0 },

		{ 0x80,0x80,0x80, 0 },
		{ 0xFF,0,   0,    0 },
		{ 0,   0xFF,0,    0 },
		{ 0xFF,0xFF,0,    0 },
		{ 0,   0,   0xFF, 0 },
		{ 0xFF,0,   0xFF, 0 },
		{ 0,   0xFF,0xFF, 0 },
		{ 0xFF,0xFF,0xFF, 0 }
	};

	PIXELFORMATDESCRIPTOR pfd;
	LOGPALETTE *pPal;
	int n, i;

	m_pOldPalette=NULL;

	n = ::GetPixelFormat(hdc);
	::DescribePixelFormat(hdc, n, sizeof(pfd), &pfd);

	if (pfd.dwFlags & PFD_NEED_PALETTE) {
		n = 1 << pfd.cColorBits;
		pPal = (PLOGPALETTE) new char[sizeof(LOGPALETTE) + n * sizeof(PALETTEENTRY)];

		ASSERT(pPal != NULL);

		pPal->palVersion = 0x300;
		pPal->palNumEntries = n;
		for (i=0; i<n; i++) {
			pPal->palPalEntry[i].peRed =
					ComponentFromIndex(i, pfd.cRedBits, pfd.cRedShift);
			pPal->palPalEntry[i].peGreen =
					ComponentFromIndex(i, pfd.cGreenBits, pfd.cGreenShift);
			pPal->palPalEntry[i].peBlue =
					ComponentFromIndex(i, pfd.cBlueBits, pfd.cBlueShift);
			pPal->palPalEntry[i].peFlags = 0;
		}

		/* fix up the palette to include the default GDI palette */
		if ((pfd.cColorBits == 8)                           &&
			(pfd.cRedBits   == 3) && (pfd.cRedShift   == 0) &&
			(pfd.cGreenBits == 3) && (pfd.cGreenShift == 3) &&
			(pfd.cBlueBits  == 2) && (pfd.cBlueShift  == 6)
		   ) {
			for (i = 1 ; i <= 12 ; i++)
				pPal->palPalEntry[defaultOverride[i]] = defaultPalEntry[i];
		}

		m_cPalette.CreatePalette(pPal);
		delete pPal;

		m_pOldPalette = m_pDC->SelectPalette(&m_cPalette, FALSE);
		m_pDC->RealizePalette();
	}
}

/////////////////////////////////////////////////////////////////////////////
// COpenGL メッセージ ハンドラ

void COpenGL::PreSubclassWindow() 
{
	// TODO: この位置に固有の処理を追加するか、または基本クラスを呼び出してください
	CRect myRect;
	//GetClientRect(&myRect);
	//TRACE("COpenGL::PreSubclassWindow RECT %d %d\n",myRect.Width(),myRect.Height());

	//m_pDC = new CClientDC(this);
	hdc=m_pDC->GetSafeHdc();
	setPixelFormat(hdc); //**Kosaka added this line.**
	CreateRGBPalette();
	if (!(hglrc = wglCreateContext(hdc))) exit(1); /*a2*/
	if(!wglMakeCurrent(hdc, hglrc)) exit(1); /*a3*/

	glViewport(0,0,myRect.Width(),myRect.Height());
	glEnable(GL_DEPTH_TEST);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	const int angle=40;
	const int frontPlanePosZ=100;
	const int rearPlanePosZ=1300;
	gluPerspective(angle, (float)myRect.Width()/(float)myRect.Height(), frontPlanePosZ, rearPlanePosZ);
	initializeScene();
	//SetTimer(IDS_TIMER1,20,NULL);

	//CWnd::PreSubclassWindow();
}

//void COpenGL::OnDestroy() 
//{
//	CWnd::OnDestroy();
//	
//	// TODO: この位置にメッセージ ハンドラ用のコードを追加してください
//	TRACE("COpenGL::OnDestroy\n");
//	wglMakeCurrent(NULL, NULL) ;  /*b1*/
//	wglDeleteContext(hglrc);   /*b2*/
//	if (m_pOldPalette)
//		m_pDC->SelectPalette(m_pOldPalette, FALSE);
//	if (m_pDC)
//		delete m_pDC;
//}

//void COpenGL::OnTimer(UINT nIDEvent) 
//{
//	// TODO: この位置にメッセージ ハンドラ用のコードを追加するかまたはデフォルトの処理を呼び出してください
//	static bool busy=false;
//	if (nIDEvent==IDS_TIMER1 && !busy) {
//		busy=true;
//		paintWindow();
//		busy=false;
//	}
//	
//	CWnd::OnTimer(nIDEvent);
//}

//void COpenGL::OnLButtonDown(UINT nFlags, CPoint point) 
//{
//	// TODO: この位置にメッセージ ハンドラ用のコードを追加するかまたはデフォルトの処理を呼び出してください
//	if(mousestatus==0) {
//		mousestatus=1;
//		posDown=point;
//	}
//	TRACE("COpenGL::OnLButtonDown x,y=%d %d\n",point.x,point.y);	
//	
//	CWnd::OnLButtonDown(nFlags, point);
//}

//void COpenGL::OnLButtonUp(UINT nFlags, CPoint point) 
//{
//	// TODO: この位置にメッセージ ハンドラ用のコードを追加するかまたはデフォルトの処理を呼び出してください
//	if(mousestatus==2) {
//		mousestatus=3;
//		posUp=point;
//	}
//	TRACE("COpenGL::OnLButtonUp x,y=%d %d\n",point.x,point.y);
//	
//	CWnd::OnLButtonUp(nFlags, point);
//}

//void COpenGL::OnMouseMove(UINT nFlags, CPoint point) 
//{
//	// TODO: この位置にメッセージ ハンドラ用のコードを追加するかまたはデフォルトの処理を呼び出してください
//	if (nFlags&MK_LBUTTON && 0<mousestatus) {
//		mousestatus=2;
//		posDrag=point;
//		TRACE("COpenGL::OnMouseMove x,y=%d %d\n",point.x,point.y);
//	}
//	
//	CWnd::OnMouseMove(nFlags, point);
//}

GLfloat light0pos[] = { 500.0, 1500.0, 500.0, 0.0 }; // x,y,z,d
GLfloat White[] = { 1.0, 1.0, 1.0, 1.0 }; // R,G,B,A
GLfloat Black[] = { 0.0, 0.0, 0.0, 1.0 }; // R,G,B,A
GLfloat Brown[] = { 0.3f, 0.25f, 0.0, 1.0 }; // R,G,B,A
GLfloat Cyan[] = { 0.0, 0.95f, 0.9f, 1.0 }; // R,G,B,A
GLfloat Gray[] = { 0.25f, 0.25f, 0.25f, 1.0 }; // R,G,B,A
GLfloat Red[] = { 1.0, 0.0, 0.0, 1.0 }; // R,G,B,A
GLfloat Shine[] = { 100.0}; //0.0(large highlight) ... 128.0(small highlight)

void COpenGL::paintWindow(void) 
{
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	glEnable(GL_NORMALIZE); // normarize normal vectors

	double ddip=0,drot=0,dip1,rot1;
	double ly,lxz,lx,lz;
	if (mousestatus==2) {
		ddip=(posDrag.y-posDown.y)*0.01;
		drot=-(posDrag.x-posDown.x)*0.01;
	} else if (mousestatus==3) {
		ddip=(posUp.y-posDown.y)*0.01;
		drot=-(posUp.x-posDown.x)*0.01;
		dip+=ddip;
		rot+=drot;
		if (0.4999*M_PI<dip) dip=0.4999*M_PI;
		if (dip<-0.4999*M_PI) dip=-0.4999*M_PI;
		mousestatus=0;
		ddip=0,drot=0;
	}
	dip1=dip+ddip;
	rot1=rot+drot;
	if (0.4999*M_PI<dip1) dip1=0.4999*M_PI;
	if (dip1<-0.4999*M_PI) dip1=-0.4999*M_PI;
	ly=leye*sin(dip1); lxz=leye*cos(dip1);
	lx=lxz*sin(rot1); lz=lxz*cos(rot1);
	gluLookAt(lx,ly,lz, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0);
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	glLightfv(GL_LIGHT0, GL_AMBIENT, White);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, White);
	glLightfv(GL_LIGHT0, GL_SPECULAR, White);
	glLightfv(GL_LIGHT0, GL_POSITION, light0pos); //definition of light position must be here
	glClearColor(0.1f,0,0.2f,0);
	glClear(GL_COLOR_BUFFER_BIT);
	glClear(GL_DEPTH_BUFFER_BIT); // depth check
	glMaterialfv(GL_FRONT, GL_AMBIENT, White); // Attribute of the Axes
	drawAxes();

 	renderScene();//render here
	SwapBuffers(hdc);
}

void COpenGL::initializeScene(void)
{
	tick=0;
}

///////////////////////////////////////////////////////////

polyhedron_t cube={
	8, //number of vertices of the cube
	{
		{50,50,50},{50,50,-50},{-50,50,-50},{-50,50,50},
		{50,-50,50},{50,-50,-50},{-50,-50,-50},{-50,-50,50}
	}, // coordinates of the vertices of the cube
	6, // number of faces of the cube
	{
		{4,{0,1,2,3},{0,1,0}}, {4,{0,4,5,1},{1,0,0}},
		{4,{1,5,6,2},{0,0,-1}}, {4,{2,6,7,3},{-1,0,0}},
		{4,{3,7,4,0},{0,0,1}}, {4,{7,6,5,4},{0,-1,0}}
	} // every face has four vertices and showing vertex numbers
};

void COpenGL::renderScene(void)
{
	glShadeModel(GL_SMOOTH);

	float k1=tick/200.f;
	glRotatef(k1*360.0f, 0.0, 1.0, 0.0); // Matrix A
	glTranslatef(200.0f, 0.0, 0.0);      // Matrix B
	glRotatef(k1*360.0f, 0.0, 0.0, 1.0); // Matrix C
	// Then Transformation Matrix for the cube is A*B*C

	glMaterialfv(GL_FRONT, GL_AMBIENT, Gray); //Attribute of the cube
	glMaterialfv(GL_FRONT, GL_DIFFUSE, Cyan);
	glMaterialfv(GL_FRONT, GL_SPECULAR, White);
	glMaterialfv(GL_FRONT, GL_SHININESS, Shine);
	putFlatPolyhedron(cube);

	tick++;
}


