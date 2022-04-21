import { observer } from 'mobx-react';
import React from 'react'
import { Card } from 'react-bootstrap'

interface Props {
  specialization: {
    name: string,
    description: string
  } | null
}

const SpecializationCard = observer((props: Props) => {
  if (!props.specialization) {
    return null
  }
  const { name, description } = props.specialization

  return (
    <>
    <Card border="dark" style={{ width: '13rem', height: '18rem' }}>
      <Card.Body>
        <Card.Title className="text-center">{name}</Card.Title>
        <hr style={{ background: "#309896", height: 4, width: "100%" }} />
        <Card.Text className="text-break">
          {description}
        </Card.Text>
      </Card.Body>
    </Card>
    </>
  )
});

export default SpecializationCard