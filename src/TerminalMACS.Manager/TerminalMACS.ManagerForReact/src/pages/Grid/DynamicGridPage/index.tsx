import React from 'react';
import { Button } from 'antd';
import { FullscreenOutlined, CloseOutlined } from '@ant-design/icons';
import styles from './index.less';

interface IVideoPanelProps { }

interface IVideoPanelSate {
  cardCount: number; // card数量
  selectCardIndex: number; // 选择的card索引
  hoverCardIndex: number; // 悬浮的card索引
  deviceList: Array<any>; // 设备列表
}

const MAX_CARD_COUNT = 16;

// 栅格容器
class VideoPanel extends React.Component<IVideoPanelProps, IVideoPanelSate> {
  constructor(props: Readonly<{}>) {
    super(props);
    this.state = {
      cardCount: 2,
      selectCardIndex: 0,
      hoverCardIndex: -1,
      deviceList: new Array(MAX_CARD_COUNT),
    };
  }

  componentDidMount() {

    // 初始化card列表
    for (let i = 0; i < MAX_CARD_COUNT; i += 1) {
      const tmpLst = this.state.deviceList;
      if (tmpLst[i] == null) {
        tmpLst[i] = {};
        tmpLst[i].name = 'dotNet 5';
        tmpLst[i].url = 'https://sec.ch9.ms/ch9/ddc3/cfd02421-f8ea-4bcd-bdaf-c3482ff2ddc3/BOD106_mid.mp4';
      }
      this.setState({
        deviceList: tmpLst,
      });
    }
  }

  // 清空指定设备信息
  clearDeviceInfo = (cardIndex: number) => {
    const tmpLst = this.state.deviceList;
    if (tmpLst[cardIndex] == null) {
      return;
    }
    tmpLst[cardIndex].name = '无';
    tmpLst[cardIndex].url = '';
    this.setState({
      deviceList: tmpLst,
    });
  };

  // 设置指定索引栅格视频
  setDevice = (obj: { name: any; sessionID: any; puid: any; idx: any }) => {
    const tmpLst = this.state.deviceList;
    tmpLst[this.state.selectCardIndex].name = obj.name;
    tmpLst[
      this.state.selectCardIndex
    ].url = `https://www.sbdjkpt.com:50443/h5live/indexJH.html?puid=${obj.puid}&idx=${obj.idx}`;
    this.setState({
      deviceList: tmpLst,
    });
  };

  // 设置视频全屏，退出全屏用esc键
  onClickSetFullScreen = (cardIndex: number) => {
    let iframeObjs = document.getElementsByTagName('iframe');
    let selectIfObj = iframeObjs[cardIndex];
    selectIfObj.requestFullscreen();
  };

  // 保存选择的card索引，用于显示选择的Card外边框
  onClickById = (cardId: any) => {
    this.setState({
      selectCardIndex: cardId,
    });
  };

  // 鼠标是否移入、移出card，用于显示Card下方的菜单
  onMouseOverCard = (cardId: any, isOver: boolean) => {
    if (isOver) {
      this.setState({
        hoverCardIndex: cardId,
      });
    } else {
      this.setState({
        hoverCardIndex: -1,
      });
    }
  };

  // 动态生成Grid
  createCard() {
    const res = [];
    for (let i = 0; i < this.state.cardCount * this.state.cardCount; i++) {
      res.push(
        <div
          key={i}
          className={[
            `${styles[`video_panel${this.state.cardCount}`]}`,
            `${this.state.selectCardIndex == i ? styles.video_panel_click : null}`,
          ].join(' ')}
          onClick={this.onClickById.bind(this, i)}
          onMouseOver={this.onMouseOverCard.bind(this, i, true)}
          onMouseOut={this.onMouseOverCard.bind(this, i, false)}
        >
          <div className={styles.cardCheckHelp} />
          <div className={styles.cardtitle}>
            {this.state.deviceList[i] != null ? this.state.deviceList[i].name : '无'}
            <div>
              <Button
                type="ghost" size="small"
                onClick={this.onClickSetFullScreen.bind(this, i, true)}
              >
                <FullscreenOutlined />
              </Button>
              <Button type="ghost" size="small">
                <CloseOutlined />
              </Button>
            </div>
          </div>
          <iframe
            title={this.state.deviceList[i] != null ? this.state.deviceList[i].name : '空'}
            src={this.state.deviceList[i] != null ? this.state.deviceList[i].url : ''}
            frameBorder="0"
            sandbox="allow-same-origin allow-scripts allow-popups allow-forms"
          />
          <div
            className={[
              `${styles.cardHoverMenu}`,
              `${
              this.state.hoverCardIndex !== -1 && this.state.hoverCardIndex === i
                ? styles.cardHoverMenuShow
                : null
              }`,
            ].join(' ')}
          >
            <Button type="primary" size="small">
              本地抓拍
            </Button>
            <Button type="primary" size="small">
              开启本地录像
            </Button>
            <Button type="primary" size="small">
              开启对讲
            </Button>
          </div>
        </div>,
      );
    }
    return res;
  }

  // 动态生成控制按钮
  createControlButon() {
    const res = [];
    const btnCount = 4;
    for (let i = 1; i <= btnCount; i += 1) {
      res.push(
        <Button
          key={i}
          className={styles.control_Button}
          type="primary"
          onClick={() => {
            this.changeCardCount(i);
          }}
        >
          {`${i}*${i}`}
        </Button>,
      );
    }

    return res;
  }

  // 修改显示的格子数
  changeCardCount(count: any) {
    this.setState({
      cardCount: count,
    });
  }

  render() {
    return (
      <div className={styles.main}>
        <div className={styles.top}>{this.createCard()}</div>
        <div className={styles.bottom}>{this.createControlButon()}</div>
      </div>
    );
  }
}

export default VideoPanel;
