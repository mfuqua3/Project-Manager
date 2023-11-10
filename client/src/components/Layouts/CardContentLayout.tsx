import React, {ReactNode} from "react";
import {Box, Card, CardContent, Container} from "@mui/material";

function CardContentLayout({children}: { children: ReactNode | ReactNode[] }) {
    return (
        <Container maxWidth="sm">
            <Box sx={{textAlign: 'center', paddingTop: 4}}>
                <Card sx={(theme) => ({
                    backgroundColor: theme.palette.neutral.main,
                    color: theme.palette.neutral.contrastText,
                    boxShadow: 2
                })}>
                    <CardContent>
                        {children}
                    </CardContent>
                </Card>
            </Box>
        </Container>
    )
}

export default React.memo(CardContentLayout);