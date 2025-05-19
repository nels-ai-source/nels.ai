import * as React from 'react';
import Layout from '../components/layoutempty';
import { graphql } from 'gatsby';
import KnowledgeDetailManger from '../components/views/knowledge/detail/detail';
import { BrowserRouter } from 'react-router-dom';

const KnowledgeDetailPage = ({ data }: any) => {
    return (
        <BrowserRouter>
            <Layout
                meta={data.site.siteMetadata}
                title="Home"
                link={'/knowledgedetail'}
            >
                <main style={{ height: '100%' }} className=" h-full ">
                    <KnowledgeDetailManger />
                </main>
            </Layout>
        </BrowserRouter>
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

export default KnowledgeDetailPage;
