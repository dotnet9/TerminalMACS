import React from 'react';
import styles from './index.less';
import { Map, Base, Marker, Constants, Label, InfoWindow } from 'rc-bmap';

// rc-bmp参考文档：// https://blog.csdn.net/feinifi/article/details/104026134?utm_medium=distribute.pc_relevant.none-task-blog-baidujs-1

// 官方文档：http://jser.wang/bmap/articles/start

const { Point, Size, Events } = Base;
const { ANIMATION } = Constants;
const { Title, Content } = InfoWindow;

class EMap extends React.Component<{}, {}> {
  state = {
    visible: false
  }
  handleMarkerClick = () => {
    this.setState({
      visible: true
    });
  };

  closeiInfoWindow = () => {
    console.log('in close');
    
  }
  
  componentDidMount() {}
  render() {
    const { visible } = this.state;
    return (
      <div className={styles.main}>
        <div className={styles.left}></div>
        <div className={styles.right} id="allmap">
          <Map ak="VcVr07PVfgacDUQO7ySsTFErHLx2DxmD" scrollWheelZoom zoom={15}>
            <Point name="center" lng="116.404" lat="39.915" />
            <Marker animation={ANIMATION.BOUNCE}>
              <Point lng="116.404" lat="39.915" />
              <Label>
                <Size name="offset" width="20" height="-10" />
                <Label.Content>我是文字标注1</Label.Content>
              </Label>
              <Events click={this.handleMarkerClick} />
              <InfoWindow visible={visible}>
                <Point lng="116.404" lat="39.915" />
                <Title>我是标题1</Title>
                <Content>
                  我是内容1
                </Content>
                <Events close={this.closeiInfoWindow} />
              </InfoWindow>
            </Marker>
            <Marker>
              <Point lng="117.404" lat="42.915" />
              <Label>
                <Size name="offset" width="20" height="-10" />
                <Label.Content>我是文字标注2</Label.Content>
              </Label>
              <Events click={this.handleMarkerClick} />
              <InfoWindow visible={visible}>
                <Point lng="117.404" lat="42.915" />
                <Title>我是标题2</Title>
                <Content>
                  我是内容2
                </Content>
                <Events close={this.closeiInfoWindow} />
              </InfoWindow>
            </Marker>
          </Map>
        </div>
      </div>
    );
  }
}

export default EMap;
