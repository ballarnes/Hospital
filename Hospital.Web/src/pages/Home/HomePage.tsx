import { observer } from 'mobx-react'
import React, { Suspense } from 'react'
import { Container, Row, Spinner, Tab, Tabs } from 'react-bootstrap'
import { useInjection } from '../../ioc/ioc.react'
import ownTypes from '../../ioc/ownTypes'
import HomePageStore, { TabsType } from '../../stores/pages/HomePageStore'
import { useTranslation } from 'react-i18next';


const Appointment = React.lazy(() => import('../../components/Appointment'))
const Information = React.lazy(() => import('../../components/Information'))

const HomePage = observer(() => {
  const store = useInjection<HomePageStore>(ownTypes.homePageStore);
  const { t } = useTranslation();
  
  return (
      <Container className="pt-4 pb-4">
        <Suspense fallback={<Row className="justify-content-center"><Spinner animation="border" /></Row>}>
        <Tabs
          activeKey={store.currentTab}
          onSelect={(ev)=> {store.changeTab(ev)}}
          className="mb-3"
        >
          <Tab eventKey={TabsType[TabsType.Information]} title={t('tabs.information')}>
            {store.currentTab === `${TabsType[TabsType.Information]}` && <Information />}
          </Tab>
          <Tab eventKey={TabsType[TabsType.Appointment]} title={t('tabs.appointment')}>
            {store.currentTab === `${TabsType[TabsType.Appointment]}` && <Appointment />}
          </Tab>
        </Tabs>
        </Suspense>
      </Container>
  )
});

export default HomePage;