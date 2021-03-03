import React, {useState} from "react";
import {Card, CardBody, CardHeader, Collapse} from "reactstrap";

export const CollapsibleCard = (props: {
    children?: any, header: any, initiallyExpanded?: boolean,
    expanded?: boolean, onExpandedChange?: (expanded: boolean) => void
}) => {
    const [state, setState] = useState({collapsed: !props.initiallyExpanded});

    const expanded = props.expanded !== undefined ? props.expanded : !state.collapsed;
    return <Card>
        <CardHeader style={{cursor: 'pointer'}} onClick={() => {
            if (props.onExpandedChange)
                props.onExpandedChange(!props.expanded);
            else
                setState({collapsed: !state.collapsed});
        }}>{props.header}</CardHeader>

        <Collapse isOpen={expanded}>
            <CardBody>
                {props.children}
            </CardBody>

        </Collapse>
    </Card>
}