import { observer } from 'mobx-react'
import React, { useEffect } from 'react'
import { Container, Row, Col, Spinner } from 'react-bootstrap'
import { useTranslation } from 'react-i18next'
import { useInjection } from '../../ioc/ioc.react'
import ownTypes from '../../ioc/ownTypes'
import SpecializationsPageStore from '../../stores/pages/SpecializationsPageStore'
import SpecializationCard from '../../components/SpecializationCard'


const SpecializationsPage = observer(() => {
  const store = useInjection<SpecializationsPageStore>(ownTypes.specializationsPageStore);
  const { t } = useTranslation();
  
  useEffect(() => {
    const getSpec = async () => {
      await store.init();
    }
    getSpec()
  }, [store, store.pageIndex])

  return (
    <Container className="pt-4 pb-4 justify-content-center">
      <h1 className='mb-4 text-center' >{t('specializationsPageTitle')}</h1>
      <Row className="justify-content-center">
        {store.isLoading ? (
          <Spinner animation="border" />
        ) : (
          <>
            {store.specializations?.map((specialization, key) => (
              <Col key={key} sm={6} md={4} lg={3} xl={2} className="mb-2 mt-2">
                <SpecializationCard specialization={specialization} />
              </Col>
            ))}
          </>
        )}
      </Row>
    </Container>
  )
});

export default SpecializationsPage;