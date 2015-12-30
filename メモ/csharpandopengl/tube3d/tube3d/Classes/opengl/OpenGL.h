#if !defined(AFX_OPENGL_H__EA152DEA_9FDA_43DF_9F97_9B195FD0C217__INCLUDED_)
#define AFX_OPENGL_H__EA152DEA_9FDA_43DF_9F97_9B195FD0C217__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// OpenGL.h : ヘッダー ファイル
//

/////////////////////////////////////////////////////////////////////////////
// COpenGL ウィンドウ

class COpenGL : public CObject
{
// コンストラクション
public:
	COpenGL();

// アトリビュート
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


// オペレーション
public:
	//BOOL attachWindow(int nID, CWnd* pParentWnd);
	unsigned char ComponentFromIndex(int i, UINT nbits, UINT shift);
	void CreateRGBPalette();
	void initializeScene(void);
	void paintWindow(void);
	void renderScene(void);
	void PreSubclassWindow();

// オーバーライド
	// ClassWizard は仮想関数のオーバーライドを生成します。

	//{{AFX_VIRTUAL(COpenGL)
	//protected:
	//virtual void PreSubclassWindow();
	//}}AFX_VIRTUAL

// インプリメンテーション
public:
	virtual ~COpenGL();

	// 生成されたメッセージ マップ関数
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
// Microsoft Visual C++ は前行の直前に追加の宣言を挿入します。

#endif // !defined(AFX_OPENGL_H__EA152DEA_9FDA_43DF_9F97_9B195FD0C217__INCLUDED_)
