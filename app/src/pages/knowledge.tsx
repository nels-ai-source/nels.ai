import * as React from 'react';
import Layout from '../components/layout';
import { graphql } from 'gatsby';
import { KnowledgeManager } from '../components/views/knowledge/manager';

const KnowledgePage = ({ data }: any) => {
    return (
        <Layout meta={data.site.siteMetadata} title="Home" link={'/knowledge'}>
            <main style={{ height: '100%' }} className=" h-full ">
                <KnowledgeManager />
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
            filter: { ns: { in: ["knowledge"] }, language: { eq: $language } }
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

export default KnowledgePage;
