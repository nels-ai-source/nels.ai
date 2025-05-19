import * as React from 'react';
import Layout from '../components/layout';
import { graphql } from 'gatsby';
import { ModelManager } from '../components/views/model/manager';

const ModelPage = ({ data }: any) => {
    return (
        <Layout meta={data.site.siteMetadata} title="Home" link={'/model'}>
            <main style={{ height: '100%' }} className=" h-full ">
                <ModelManager />
            </main>
        </Layout>
    );
};

export const query = graphql`
    query ($language: String!) {
        site {
            siteMetadata {
                description
                title
            }
        }
        locales: allLocale(
            filter: { ns: { in: ["model"] }, language: { eq: $language } }
        ) {
            edges {
                node {
                    ns
                    data
                    language
                }
            }
        }
    }
`;

export default ModelPage;
