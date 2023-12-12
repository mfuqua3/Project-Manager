import React from "react";
import {Field, Formik, FormikHelpers, Form} from 'formik';
import {Typography, TextField, Grid, Button, Box, Divider} from '@mui/material';
import { styled } from '@mui/system';
import {useLogger} from "../../utils/logging";


// Styled container to handle margins and border
const CustomContainer = styled(Box)(({ theme }) => ({
  margin: theme.spacing(2),
  border: '1px solid #ddd',
  borderRadius: theme.shape.borderRadius,
  padding: theme.spacing(2)
}));

function NewProject() {
    const logger = useLogger(NewProject);
    function onSubmit(values: { projectName: string }, _: FormikHelpers<{ projectName: string }>) {
        logger.debug(values.projectName);
    }

    function onCancelled() {
        logger.debug("cancelled");
    }

    return (
        <Formik
           initialValues={{projectName: ''}}
           onSubmit={onSubmit}
        >
           {({isSubmitting}) => (
            <Form>
                <CustomContainer>
                    <Typography variant="h6" gutterBottom>
                        Launch a New Project
                    </Typography>
                    <Divider />
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <Field name="projectName">
                                {({field}: { field: any}) => (
                                    <TextField
                                        {...field}
                                        label="Project Name"
                                        required
                                        fullWidth
                                        variant="outlined"
                                    />
                                )}
                            </Field>
                        </Grid>
                        <Grid item container justifyContent="flex-end" spacing={2}>
                            <Grid item>
                                <Button variant="outlined" onClick={onCancelled}>
                                    Cancel
                                </Button>
                            </Grid>
                            <Grid item>
                                <Button variant="contained" color="primary" type="submit" disabled={isSubmitting}>
                                    Submit
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </CustomContainer>
            </Form>
          )}
        </Formik>
    );
}

export default React.memo(NewProject);