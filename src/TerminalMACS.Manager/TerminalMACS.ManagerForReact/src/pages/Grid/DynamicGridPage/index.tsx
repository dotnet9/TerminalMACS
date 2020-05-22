import React from 'react';
import styles from './index.less';
import { Button, Card } from 'antd';

interface IVideoPanelProps {}

interface IVideoPanelSate {
  cardCount: number;
}

class VideoPanel extends React.Component<IVideoPanelProps, IVideoPanelSate> {
  constructor(props: Readonly<{}>) {
    super(props);
    this.state = {
      cardCount: 1,
    };
  }

  // 动态生成Grid
  createCard() {
    var res = [];
    for (var i = 0; i < this.state.cardCount * this.state.cardCount; i++) {
      res.push(<Card className={styles['video_panel' + this.state.cardCount]} />);
    }
    return res;
  }

  // 动态生成控制按钮
  createControlButon() {
    var res = [];
    const btnCount = 4;
    for (let i = 1; i <= btnCount; i++) {
      res.push(
        <Button
          key={i}
          className={styles['control_Button']}
          type="primary"
          onClick={() => {
            this.changeCardCount(i);
          }}
        >
          {i + '*' + i}
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
        <div className={styles.main_child}>
          <div className={styles.left}>
            <div className={styles.left_top}></div>
            <div className={styles.left_bottom}></div>
          </div>
          <div className={styles.right}>
            <div className={styles.right_top}>{this.createCard()}</div>
            <div className={styles.right_bottom}>{this.createControlButon()}</div>
          </div>
        </div>
      </div>
    );
  }
}

export default VideoPanel;
